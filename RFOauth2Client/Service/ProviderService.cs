using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFAuth.IServices;
using RFBase.Libs;
using RFEntities.Entities;
using RFIServices.IServices;
using RFOauth2Client.Entities;
using RFOauth2Client.Exceptions;
using RFOauth2Client.IServices;
using RFRBAC.IServices;
using RFRegisterService.Attributes;
using RFUserEmailVerified.Entities;
using RFUserEmailVerified.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace RFOauth2Client.Service;

[RegisterService]
public class ProviderService(IServiceProvider serviceProvider)
    : IProviderService
{
    private static readonly Dictionary<string, Dictionary<string, TokenResponse>> UserTokens = [];

    static private List<Provider>? ConfigurationProviders = null;

    public IUserService UserService { get => serviceProvider.GetRequiredService<IUserService>(); }
    public IDeviceService DeviceService { get => serviceProvider.GetRequiredService<IDeviceService>(); }
    public ILoginService LoginService { get => serviceProvider.GetRequiredService<ILoginService>(); }
    public IUserEmailVerifiedService UserEmailVerifiedService { get => serviceProvider.GetRequiredService<IUserEmailVerifiedService>(); }
    public IRoleXUserService RoleXUserService { get => serviceProvider.GetRequiredService<IRoleXUserService>(); }
    public IUserTypeService UserTypeService { get => serviceProvider.GetRequiredService<IUserTypeService>(); }

    public async Task<IEnumerable<Provider>> GetListAsync()
    {
        if (ConfigurationProviders == null)
        {
            var configurationProviders = new List<Provider>();
            var configuration = serviceProvider.GetService<IConfiguration>();
            var providersSection = configuration?.GetSection("OAuth2Providers");

            if (providersSection != null)
            {
                foreach (var child in providersSection.GetChildren())
                {
                    var provider = new Provider
                    {
                        Name = child["name"] ?? child.Key,
                        DisplayName = child["displayName"] ?? child["name"] ?? providersSection.Key,
                        IsEnabled = bool.TryParse(child["isEnabled"] ?? "true", out var isEnabled) && isEnabled,
                        Client = child.GetRequiredSection("client").Get<Client>()
                            ?? throw new NoClientSectionInOAuth2ProvidersConfigurationException(),
                        Endpoints = child.GetSection("endpoints").Get<Dictionary<string, Entities.Endpoint>>() ?? [],
                        RolesSources = child.GetSection("roles").Get<List<RolesSource>>(),
                        Features = child.GetSection("features").Get<Features>(),
                    };

                    CheckEndpointConfiguration(provider, child, "authorize");
                    CheckEndpointConfiguration(provider, child, "token");
                    CheckEndpointConfiguration(provider, child, "userInfo");

                    foreach (var kv in provider.Endpoints)
                    {
                        var name = kv.Key;
                        var endpoint = kv.Value;
                        
                        endpoint.Name = name;
                        if (name == "authorize")
                        {
                            endpoint.Method ??= Method.GET;
                            endpoint.AuthorizationHeader ??= false;
                            endpoint.ClientIdInQuery ??= true;
                            endpoint.RedirectUriInQuery ??= true;

                            if (!(endpoint.Query?.ContainsKey("scope") ?? true))
                            {
                                var scope = child.GetSection("scope")?.Get<string>()
                                    ?? "openid email profile";

                                endpoint.AddQueryParameterIfNotExists("scope", scope);
                            }
                        }
                        else if (name == "token")
                        {
                            endpoint.Method ??= Method.POST;
                            endpoint.AuthorizationHeader ??= false;

                            endpoint.ContentType ??= ContentType.FormUrlEncoded;
                            endpoint.ClientIdInBody??= true;
                            endpoint.ClientSecretInBody ??= true;
                            endpoint.RedirectUriInBody ??= true;

                            endpoint.AddBodyParameter("grant_type", "authorization_code");
                            endpoint.AddBodyParameter("response_type", "code");
                        }
                        else if (name == "logout")
                        {
                            endpoint.Method ??= Method.GET;
                        }

                        endpoint.URL ??= $"/{name}";
                        endpoint.Method ??= Method.POST;
                        endpoint.AuthorizationHeader ??= true;

                        endpoint.ClientIdInQuery ??= false;
                        endpoint.RedirectUriInQuery ??= false;
                        endpoint.ClientSecretInQuery ??= false;
                        endpoint.RefreshTokenInQuery ??= false;

                        endpoint.ContentType ??= ContentType.FormUrlEncoded;
                        endpoint.ClientIdInBody ??= false;
                        endpoint.RedirectUriInBody ??= false;
                        endpoint.ClientSecretInBody ??= false;
                        endpoint.RefreshTokenInBody ??= false;
                    }

                    configurationProviders.Add(provider);
                }
            }

            ConfigurationProviders = configurationProviders;
        }

        return ConfigurationProviders;
    }

    public static void CheckEndpointConfiguration(Provider provider, IConfigurationSection child, string endpointName)
    {
        if (provider.Endpoints.ContainsKey(endpointName))
            return;

        var section = child.GetSection($"endpoints:{endpointName}");
        if (!section.Exists())
        {
            provider.Endpoints[endpointName] = new Entities.Endpoint()
            {
                URL = $"/{endpointName}",
            };

            return;
        }

        var raw = section.Value;
        if (bool.TryParse(raw, out var flag))
        {
            if (!flag)
                return;

            provider.Endpoints[endpointName] = new Entities.Endpoint()
            {
                URL = $"/{endpointName}",
            };
        }

        provider.Endpoints[endpointName] = new Entities.Endpoint()
        {
            URL = raw,
        };
    }

    public async Task<IEnumerable<Provider>> GetListAuthorizeAsync()
        => (await GetListAsync())
            .Where(provider => provider.IsEnabled
                && !string.IsNullOrEmpty(provider.Name)
                && provider.Client is not null
                && !string.IsNullOrEmpty(provider.Client.ClientId)
                && !string.IsNullOrEmpty(provider.Client.ClientSecret)
                && !string.IsNullOrEmpty(provider.Client.RedirectUri)
                && provider.Endpoints.ContainsKey("token")
                && provider.Endpoints.TryGetValue("authorize", out var authorizeEndpoint)
                && authorizeEndpoint != null
                && !string.IsNullOrEmpty(authorizeEndpoint.URL)
            );
    
    public async Task<Provider?> GetSingleOrDefaultByNameAsync(string name)
        => (await GetListAsync())
            .FirstOrDefault(p => p.Name == name);

    public async Task<SessionResponse?> CallbackAsync(string name, string actionName, DataDictionary? data, HttpRequest request)
    {
        var provider = await GetSingleOrDefaultByNameAsync(name)
            ?? throw new ProviderNotFoundException(name);

        if (actionName == "authorize")
            return await CallbackAuthorizeAsync(provider, data, request);
        
        throw new ActionNotSupportedForProviderException(actionName, name);
    }

    public static async Task<TokenResponse?> GetToken(Provider provider, string code)
    {
        if (provider == null)
            throw new ProviderIsNullException();

        if (string.IsNullOrEmpty(code))
            throw new NoCodeProvidedInDataException();

        if (!provider.Endpoints.TryGetValue("token", out var tokenEndpoint)
            || tokenEndpoint == null)
            throw new ActionNotFoundInProviderException("token", provider.Name);

        tokenEndpoint.AddBodyParameter("code", code);
        var res = await tokenEndpoint.Request(provider, null);

        var body = await res.Content.ReadAsStringAsync();
        if (res.IsSuccessStatusCode)
        {
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(body);
            return tokenResponse;
        }

        var message = body;
        try
        {
            var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;

            message = root.GetProperty("error_description").ToString()
                ?? root.GetProperty("error_reason").ToString()
                ?? root.GetProperty("error").ToString();
        } catch(JsonException) { }

        throw new ErrorRetrivingAccessTokenException(message);
    }

    public static async Task<T?> Request<T>(Provider provider, Entities.Endpoint endpoint, TokenResponse tokenResponse)
    {
        var response = await endpoint.Request(provider, tokenResponse);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<T>(body);

        return data;
    }

    public static async Task<UserInfo?> GetUserInfo(Provider provider, TokenResponse tokenResponse)
    {
        if (!provider.Endpoints.TryGetValue("userInfo", out var userInfoEndpoint)
            || userInfoEndpoint == null)
            throw new NoUserInfoInProviderException(provider.Name);

        return await Request<UserInfo>(provider, userInfoEndpoint, tokenResponse);
    }

    public async Task<SessionResponse?> CallbackAuthorizeAsync(Provider provider, DataDictionary? data, HttpRequest request)
    {
        var tokenResponse = await GetToken(provider, data?.GetString("code") ?? "")
            ?? throw new NoAccessTokenReceivedException();
        if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            throw new NoAccessTokenReceivedException();

        var userInfo = await GetUserInfo(provider, tokenResponse)
            ?? throw new NoUserInfoException();

        var username = RFString.FirstNonEmpty(userInfo.PreferredUsername, userInfo.Username, userInfo.Email, userInfo.Sub, userInfo.Name);
        var userId = await UserService.GetSingleIdOrDefaultByUsernameAsync(username);

        var accessToken = tokenResponse.AccessToken;

        if (!UserTokens.ContainsKey(provider.Name))
            UserTokens[provider.Name] = [];

        var userTokens = UserTokens[provider.Name];
        userTokens[username] = tokenResponse;

        if (!userId.HasValue)
        {
            if (!provider.Features?.AllowSelfRegistration ?? false)
                throw new UserNotFoundException();

            var user = await RegisterUser(userInfo, username);
            userId = user.Id;
        }
        
        if (provider.Features?.MandatoryRoles ?? false)
            await SetUserRoles(provider, tokenResponse, userId.Value);

        var deviceId = await DeviceService.GetSingleByTokenOrCreateAsync(data?.GetString("deviceToken") ?? "");
        var userDeviceDTO = new UserIdAndDeviceIdDTO { UserId = userId!.Value, DeviceId = deviceId.Id };

        var httpContext = serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext;

        var clientData = new DataDictionary {
            { "ip", request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? httpContext?.Connection.RemoteIpAddress?.ToString() },
            { "userAgent", request.Headers.UserAgent.ToString() },
            { "service", "oauth2"},
        };

        var session = await LoginService.LoginAsync(userDeviceDTO, provider.Name, clientData);

        return new SessionResponse(session);
    }

    public async Task<User> RegisterUser(UserInfo userInfo, string username)
    {
        var systemUserId = await UserService.GetCurrentOrSystemUserIdAsync();
        var displayName = RFString.FirstNonEmpty(userInfo.FullName, userInfo.Name, $"{userInfo.GivenName} {userInfo.FamilyName}".Trim());
        var user = await UserService.CreateAsync(new User
        {
            Username = username,
            DisplayName = displayName,
            TypeId = await UserTypeService.GetSingleIdByNameAsync("user"),
            CreatedById = systemUserId,
            UpdatedById = systemUserId,
        });

        if (!string.IsNullOrEmpty(userInfo.Email))
        {
            var email = await UserEmailVerifiedService.GetSingleOrDefaultByUserIdAsync(user.Id);
            if (email is null)
            {
                await UserEmailVerifiedService.CreateAsync(new UserEmailVerified
                {
                    UserId = user.Id,
                    Email = userInfo.Email,
                    IsVerified = userInfo.EmailVerified,
                    CreatedById = systemUserId,
                    UpdatedById = systemUserId,
                });
            }
            else if (email.Email != userInfo.Email || email.IsVerified != userInfo.EmailVerified)
            {
                await UserEmailVerifiedService.UpdateByIdAsync(
                    email.Id,
                    new DataDictionary {
                        { "Email", userInfo.Email },
                        { "IsVerified", userInfo.EmailVerified },
                        { "UpdatedById", systemUserId },
                    }
                );
            }
        }

        return user;
    }

    public async Task SetUserRoles(Provider provider, TokenResponse tokenResponse, long userId)
    {
        List<string> roles = [];
        var rolesSources = provider.RolesSources ?? [];
        foreach (var rolesSource in rolesSources)
        {
            JsonElement? jsonData = null;
            if (rolesSource.Source == "token")
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(tokenResponse.AccessToken);
                jsonData = JsonSerializer.Deserialize<JsonElement>(jwt.Payload.SerializeToJson());
            }

            if (jsonData is not null)
            {
                var rolesSection = jsonData;
                if (!string.IsNullOrEmpty(rolesSource.Path))
                {
                    var path = rolesSource.Path.Split('.');
                    foreach (var step in path)
                    {
                        if (!rolesSection.Value.TryGetProperty(step, out var nextSection))
                        {
                            rolesSection = null;
                            break;
                        }

                        rolesSection = nextSection;
                    }
                }

                if (rolesSection is not null)
                {
                    roles.AddRange(rolesSection.Value
                        .EnumerateArray()
                        .Select(r => r.GetString())
                        .Where(r => !string.IsNullOrEmpty(r))
                        .Select(r => r!.Trim()));
                }
            }
        }

        if (roles.Count != 0)
            await RoleXUserService.SetAllRolesForUserIdAsync(roles, userId);
    }

    public async Task<bool> Logout(Provider provider, string username)
    {
        if (!provider.Endpoints.TryGetValue("logout", out var logoutEndpoint)
            || logoutEndpoint == null
            || !UserTokens.TryGetValue(provider.Name, out var userTokens)
            || !userTokens.TryGetValue(username, out var tokenResponse)
        )
            return false;

        await logoutEndpoint.Request(provider, tokenResponse);
        return true;
    }
}
