using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RFBase.Exceptions;
using RFBase.Libs;
using RFOauth2Client.Entities;
using RFOauth2Client.Exceptions;
using RFOauth2Client.IServices;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RFOauth2Client.Service
{
    public class ProviderService(IServiceProvider serviceProvider)
        : IProviderService
    {
        static private List<Provider>? ConfigurationProviders = null;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IEnumerable<Provider>> GetListAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
                        configurationProviders.Add(new Provider {
                            Name = child["name"] ?? "",
                            Disabled = bool.TryParse(child["disabled"], out var disabled) && disabled,
                            ClientId = child["clientId"] ?? "",
                            ClientSecret = child["clientSecret"] ?? "",
                            Actions = new DataDictionary(child.GetSection("actions")),
                        });
                    }
                }

                ConfigurationProviders = configurationProviders;
            }

            return ConfigurationProviders;
        }

        public async Task<IEnumerable<AuthorizeProvider>> GetListAuthorizeAsync()
        {
            return (await GetListAsync())
                .Where(provider => !provider.Disabled
                    && !string.IsNullOrEmpty(provider.Name)
                    && !string.IsNullOrEmpty(provider.ClientId)
                    && !string.IsNullOrEmpty(provider.ClientSecret)
                    && provider.Actions.ContainsKey("token")
                    && provider.Actions.TryGet<DataDictionary>("authorize", out var authorizeAction)
                    && authorizeAction != null
                    && !string.IsNullOrEmpty(authorizeAction.GetString("redirect_uri"))
                    && !string.IsNullOrEmpty(authorizeAction.GetString("url"))
                )
                .Select(provider => {
                    provider.Actions.TryGet<DataDictionary>("authorize", out var authorizeAction);
                    return new AuthorizeProvider
                    {
                        Name = provider.Name,
                        Label = authorizeAction.GetString("label") ?? "Login",
                        ClientId = provider.ClientId,
                        ResponseType = authorizeAction.GetString("response_type") ?? "code",
                        RedirectUri = authorizeAction.GetString("redirect_uri")!,
                        Scope = authorizeAction.GetString("scope") ?? string.Empty,
                        Url = authorizeAction.GetString("url") ?? string.Empty
                    };
                });
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

            if (!provider.Actions.TryGet<DataDictionary>("token", out var tokenAction)
                || tokenAction == null)
                throw new ActionNotFoundInProviderException("token", provider.Name);
            
            var tokenUrl = tokenAction.GetString("url");
            if (string.IsNullOrEmpty(tokenUrl))
                throw new NoTokenURLInActionException();

            var redirect_uri = tokenAction.GetString("redirect_uri");
            if (string.IsNullOrEmpty(redirect_uri))
            {
                provider.Actions.TryGet<DataDictionary>("authorize", out var authorizeAction);
                if (authorizeAction.TryGetNotNullOrEmptyString("redirect_uri", out var newRedirectUri))
                    redirect_uri = newRedirectUri;

                if (string.IsNullOrEmpty(redirect_uri))
                    throw new NoRedirectURIInActionException();
            }

            if (string.IsNullOrEmpty(code))
                throw new NoCodeProvidedInDataException();

            var queryParams = new Dictionary<string, string>
            {
                { "client_id", provider.ClientId },
                { "client_secret", provider.ClientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirect_uri }
            };

            var content = new FormUrlEncodedContent(queryParams);

            var client = new HttpClient();
            var res = await client.PostAsync(tokenUrl, content);
            var body = await res.Content.ReadAsStringAsync();

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(body);
            var accessToken = tokenResponse?.AccessToken;

            return accessToken;
        }

        public static async Task<HttpResponseMessage> Get(DataDictionary actionData, string accessToken)
        {
            var userInfoUrl = actionData.GetString("url");
            if (string.IsNullOrEmpty(userInfoUrl))
                throw new NoUserInfoInActionException();

            var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, userInfoUrl);
            userInfoRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var client = new HttpClient();
            var response = await client.SendAsync(userInfoRequest);

            return response;
        }

        public static async Task<T?> Get<T>(DataDictionary actionData, string accessToken)
        {
            var response = await Get(actionData, accessToken);
            var body = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<T>(body);

            return data;
        }

        public static async Task<UserInfo?> GetUserInfo(Provider provider, string accessToken)
        {
            if (!provider.Actions.TryGet<DataDictionary>("userInfo", out var userInfoAction)
                || userInfoAction == null)
                throw new NoUserInfoInProviderException(provider.Name);

            return await Get<UserInfo>(userInfoAction, accessToken);
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
}
