using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RFBase.Exceptions;
using RFBase.Libs;
using RFOauth2Client.Entities;
using RFOauth2Client.Exceptions;
using RFOauth2Client.IServices;
using RFServices.Attributes;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RFOauth2Client.Service;

[RegisterService]
public class ProviderService(IServiceProvider serviceProvider)
    : IProviderService
{
    static private List<Provider>? ConfigurationProviders = null;

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
                    var client = child as Client;
                    var provider = new Provider
                    {
                        Name = child["name"] ?? providersSection.Key,
                        DisplayName = child["displayName"] ?? child["name"] ?? providersSection.Key,
                        IsEnabled = bool.TryParse(child["isEnabled"] ?? "true", out var isEnabled) && isEnabled,
                        Client = child.GetRequiredSection("client").Get<Client>()
                            ?? throw new Exception("No client section in OAuth2Providers configuration"),
                        Endpoints = child.GetSection("endpoints").Get<Dictionary<string, Endpoint>>()
                            ?? throw new Exception("No endpoints section in OAuth2Providers configuration"),
                        Roles = child.GetSection("roles") as Roles,
                        Features = child.GetSection("features") as Features,
                    };
                    configurationProviders.Add(provider);
                }
            }

            ConfigurationProviders = configurationProviders;
        }

        return ConfigurationProviders;
    }

    public async Task<IEnumerable<Provider>> GetListAuthorizeAsync()
    {
        return (await GetListAsync())
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
    }
    
    public async Task<Provider?> GetSingleOrDefaultByNameAsync(string name)
    {
        return (await GetListAsync())
            .FirstOrDefault(p => p.Name == name);
    }

    public async Task<object?> CallbackAsync(string name, string actionName, DataDictionary? data)
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

        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(body);
        var accessToken = tokenResponse?.AccessToken;

        return accessToken;
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

    public async Task<object?> CallbackAuthorizeAsync(Provider provider, DataDictionary? data)
    {
        var accessToken = await GetToken(provider, data?.GetString("code") ?? "");
        if (string.IsNullOrEmpty(accessToken))
            throw new HttpException(400, $"No access token received.");

        var userInfo = await GetUserInfo(provider, accessToken)
            ?? throw new HttpException(400, $"No user info.");

        throw new NotImplementedException();

        /* var evtData = new DataDictionary {
            { "Username", RFString.FirstNonEmpty(userInfo.Username, userInfo.Email, userInfo.Name) },
            { "FullName", RFString.FirstNonEmpty(userInfo.FullName, userInfo.Name, userInfo.Username, userInfo.Email) },
            { "Email", userInfo.Email },
            { "DeviceToken", data?.GetString("deviceToken") },
        };

        if (provider.Actions.TryGet<DataDictionary>("token", out var tokenAction)
            && tokenAction != null)
        {
            var selfServiceRegistration = tokenAction.GetBool("selfServiceRegistration");
            if (selfServiceRegistration != null)
            {
                evtData["SelfServiceRegistration"] = selfServiceRegistration;
            }

            var mandatoryRoles = tokenAction.GetBool("mandatoryRoles");
            if (mandatoryRoles != null)
            {
                evtData["MandatoryRoles"] = mandatoryRoles;
            }

            List<string> roles = [];
            if (tokenAction.TryGet<DataDictionary>("rolesFrom", out var rolesFrom)
                && rolesFrom != null)
            {
                List<string>? rolesFromAccessTokenList;

                if (rolesFrom.TryGet<List<string>>("access_token", out var access_tokenList) && access_tokenList != null)
                    rolesFromAccessTokenList = access_tokenList;
                else if (rolesFrom.TryGet<string>("access_token", out var access_token) && !string.IsNullOrEmpty(access_token))
                    rolesFromAccessTokenList = [access_token];
                else
                    rolesFromAccessTokenList = null;

                if (rolesFromAccessTokenList != null)
                {
                    foreach (var rolesFromAccessToken in rolesFromAccessTokenList)
                    {
                        if (string.IsNullOrEmpty(rolesFromAccessToken))
                            continue;

                        var handler = new JwtSecurityTokenHandler();
                        var jwt = handler.ReadJwtToken(accessToken);
                        var payload = JsonSerializer.Deserialize<JsonElement>(jwt.Payload.SerializeToJson());
                        var path = rolesFromAccessToken.Split('.');
                        var section = payload;

                        foreach (var part in path)
                        {
                            if (!section.TryGetProperty(part, out var nextSection))
                                break;

                            section = nextSection;
                        }

                        roles.AddRange(section
                            .EnumerateArray()
                            .Select(r => r.GetString())
                            .Where(r => !string.IsNullOrEmpty(r))
                            .Select(r => r!.Trim())
                        );
                    }
                }
            }

            evtData["Roles"] = roles;
        }

        var evtOptions = new DataDictionary { { "Data", evtData } };

        if (evtOptions.TryGet<object?>("Response", out var response))
            return response;
        else
        {
            var logger = serviceProvider.GetService<ILoggerService>();
            logger?.AddWarningAsync(
                "RFOAuth2Client",
                "No 'Result' for 'login'. Check for login decorators installed."
            );
        }

        return null; */
    }
}
