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
using System.Net.Http.Headers;
using System.Text.Json;

namespace RFOauth2Client.Service;

[RegisterService]
public class ProviderService(IServiceProvider serviceProvider)
    : IProviderService
{
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
                        Endpoints = child.GetSection("endpoints").Get<Dictionary<string, Endpoint>>()
                            ?? throw new NoEndpointsSectionInOAuth2ProvidersConfigurationException(),
                        RolesSources = child.GetSection("roles").Get<List<RolesSource>>(),
                        Features = child.GetSection("features").Get<Features>(),
                    };
                    configurationProviders.Add(provider);
                }
            }

            ConfigurationProviders = configurationProviders;
        }

        return ConfigurationProviders;
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

    public async Task<SessionResponse?> CallbackAsync(string name, string actionName, DataDictionary? data)
    {
        var provider = await GetSingleOrDefaultByNameAsync(name)
            ?? throw new ProviderNotFoundException(name);

        if (actionName == "authorize")
            return await CallbackAuthorizeAsync(provider, data);
        
        throw new ActionNotSupportedForProviderException(actionName, name);
    }

    public static async Task<string?> GetToken(Provider provider, string code)
    {
        if (provider == null)
            throw new ProviderIsNullException();

        if (string.IsNullOrEmpty(code))
            throw new NoCodeProvidedInDataException();

        if (!provider.Endpoints.TryGetValue("token", out var tokenEndpoint)
            || tokenEndpoint == null)
            throw new ActionNotFoundInProviderException("token", provider.Name);
        
        var tokenUrl = tokenEndpoint.URL;
        if (string.IsNullOrEmpty(tokenUrl))
            throw new NoTokenURLInActionException();

        var redirectUri = provider.Client.RedirectUri;
        if (string.IsNullOrEmpty(redirectUri))
        {
            if (string.IsNullOrEmpty(redirectUri))
                throw new NoRedirectURIInActionException();
        }

        var queryParams = new Dictionary<string, string>
        {
            { "client_id", provider.Client.ClientId },
            { "client_secret", provider.Client.ClientSecret },
            { "code", code },
            { "grant_type", "authorization_code" },
            { "redirect_uri", redirectUri }
        };

        var content = new FormUrlEncodedContent(queryParams);

        var client = new HttpClient();
        var res = await client.PostAsync(tokenUrl, content);
        var body = await res.Content.ReadAsStringAsync();
        if (res.IsSuccessStatusCode)
        {
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(body);
            var accessToken = tokenResponse?.AccessToken;

            return accessToken;
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

    public static async Task<HttpResponseMessage> Get(Provider provider, Endpoint endpoint, string accessToken)
    {
        var userInfoUrl = endpoint.GetFullURL(provider);
        if (string.IsNullOrEmpty(userInfoUrl))
            throw new NoUserInfoInActionException();

        var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, userInfoUrl);
        userInfoRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var client = new HttpClient();
        var response = await client.SendAsync(userInfoRequest);

        return response;
    }

    public static async Task<T?> Get<T>(Provider provider, Endpoint endpoint, string accessToken)
    {
        var response = await Get(provider, endpoint, accessToken);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<T>(body);

        return data;
    }

    public static async Task<UserInfo?> GetUserInfo(Provider provider, string accessToken)
    {
        if (!provider.Endpoints.TryGetValue("userInfo", out var userInfoEndpoint)
            || userInfoEndpoint == null)
            throw new NoUserInfoInProviderException(provider.Name);

        return await Get<UserInfo>(provider, userInfoEndpoint, accessToken);
    }

    public async Task<SessionResponse?> CallbackAuthorizeAsync(Provider provider, DataDictionary? data)
    {
        var token = await GetToken(provider, data?.GetString("code") ?? "");
        if (string.IsNullOrEmpty(token))
            throw new NoAccessTokenReceivedException();

        var userInfo = await GetUserInfo(provider, token)
            ?? throw new NoUserInfoException();

        var username = RFString.FirstNonEmpty(userInfo.PreferredUsername, userInfo.Username, userInfo.Email, userInfo.Sub, userInfo.Name);
        var userId = await UserService.GetSingleIdOrDefaultByUsernameAsync(username);

        if (!userId.HasValue)
        {
            if (!provider.Features?.AllowSelfRegistration ?? false)
                throw new UserNotFoundException();

            var user = await RegisterUser(userInfo, username);
            userId = user.Id;
        }
        
        if (provider.Features?.MandatoryRoles ?? false)
            await SetUserRoles(provider, token, userId.Value);

        var deviceId = await DeviceService.GetSingleByTokenOrCreateAsync(data?.GetString("deviceToken") ?? "");
        var session = await LoginService.LoginAsync(new UserIdAndDeviceIdDTO { UserId = userId!.Value, DeviceId = deviceId.Id });

        return new SessionResponse(session);
    }

    public async Task<User> RegisterUser(UserInfo userInfo, string username)
    {
        var displayName = RFString.FirstNonEmpty(userInfo.FullName, userInfo.Name, $"{userInfo.GivenName} {userInfo.FamilyName}".Trim());
        var user = await UserService.CreateAsync(new User
        {
            Username = username,
            DisplayName = displayName,
            TypeId = await UserTypeService.GetSingleIdByNameAsync("system"),
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
                });
            }
            else if (email.Email != userInfo.Email || email.IsVerified != userInfo.EmailVerified)
            {
                await UserEmailVerifiedService.UpdateByIdAsync(
                    email.Id,
                    new DataDictionary {
                        { "Email", userInfo.Email },
                        { "IsVerified", userInfo.EmailVerified },
                });
            }
        }

        return user;
    }

    public async Task SetUserRoles(Provider provider, string token, long userId)
    {
        List<string> roles = [];
        var rolesSources = provider.RolesSources ?? [];
        foreach (var rolesSource in rolesSources)
        {
            JsonElement? rolesJsonData = null;
            if (rolesSource.Endpoint == "token")
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                rolesJsonData = JsonSerializer.Deserialize<JsonElement>(jwt.Payload.SerializeToJson());
            }

            if (rolesJsonData is not null)
            {
                if (!string.IsNullOrEmpty(rolesSource.Path))
                {
                    var path = rolesSource.Path.Split('.');
                    foreach (var part in path)
                    {
                        if (!rolesJsonData.Value.TryGetProperty(part, out var nextSection))
                            break;

                        rolesJsonData = nextSection;
                    }
                }

                roles.AddRange(rolesJsonData.Value
                    .EnumerateArray()
                    .Select(r => r.GetString())
                    .Where(r => !string.IsNullOrEmpty(r))
                    .Select(r => r!.Trim()));
            }
        }

        if (roles.Count != 0)
            await RoleXUserService.SetAllRolesForUserIdAsync(roles, userId);
    }
}
