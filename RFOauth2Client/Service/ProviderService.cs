using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RFHttpExceptions.Exceptions;
using RFOauth2Client.DTO;
using RFOauth2Client.IServices;
using RFService.IServices;
using RFService.Libs;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RFOauth2Client.Service
{
    public class ProviderService(IServiceProvider serviceProvider)
        : IProviderService
    {
        public async Task<IEnumerable<Provider>> GetListAsync()
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            return configuration
                ?.GetSection("OAuth2Providers")
                ?.Get<Dictionary<string, ProviderData>>()
                ?.Select(kvp => new Provider
                {
                    Name = kvp.Key,
                    ClientId = kvp.Value.ClientId,
                    ClientSecret = kvp.Value.ClientSecret,
                    Actions = kvp.Value.Actions
                })
                ?? [];
        }

        public async Task<IEnumerable<AuthorizeProvider>> GetListAuthorizeAsync()
        {
            return (await GetListAsync())
                .Where(provider => !string.IsNullOrEmpty(provider.Name)
                    && !string.IsNullOrEmpty(provider.ClientId)
                    && !string.IsNullOrEmpty(provider.ClientSecret)
                    && provider.Actions != null
                    && provider.Actions.GetSection("token").Exists()
                    && provider.Actions.GetSection("authorize").Exists()
                    && !string.IsNullOrEmpty(provider.Actions.GetSection("authorize")["redirect_uri"])
                    && !string.IsNullOrEmpty(provider.Actions.GetSection("authorize")["url"])
                )
                .Select(provider => {
                    var actionData = provider.Actions!.GetSection("authorize");
                    return new AuthorizeProvider
                    {
                        Label = actionData["label"] ?? "Login",
                        ClientId = provider.ClientId,
                        ResponseType = actionData["response_type"] ?? "code",
                        RedirectUri = actionData["redirect_uri"]!,
                        Scope = actionData["scope"] ?? string.Empty,
                        Url = actionData["url"] ?? string.Empty
                    };
                });

        }

        public async Task<Provider?> GetSingleOrDefaultByNameAsync(string name)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var providerData = configuration
                ?.GetSection("OAuth2Providers")
                ?.GetSection(name)
                ?.Get<ProviderData>();

            if (providerData == null)
                return null;

            return new Provider
            {
                Name = name,
                ClientId = providerData.ClientId,
                ClientSecret = providerData.ClientSecret,
                Actions = providerData.Actions
            };
        }

        public async Task<object?> CallbackAsync(string name, string actionName, DataDictionary? data)
        {
            var provider = await GetSingleOrDefaultByNameAsync(name)
                ?? throw new HttpException(404, $"Provider '{name}' not found.");

            if (actionName == "authorize")
                return await CallbackAuthorizeAsync(provider, data);
            
            throw new HttpException(400, $"Action '{actionName}' is not supported for provider '{name}'.");
        }

        public static async Task<string?> GetToken(ProviderData provider, string code)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider), "Provider cannot be null.");

            if (provider.Actions == null)
                throw new ArgumentNullException(nameof(provider.Actions), "Provider actions is null.");  

            var actionData = provider.Actions.GetSection("token");
            if (actionData == null || !actionData.Exists())
                throw new HttpException(404, $"Action 'token' not found in provider.");
            
            var tokenUrl = actionData["url"];
            if (string.IsNullOrEmpty(tokenUrl))
                throw new HttpException(400, $"No token URL in action.");

            var redirect_uri = actionData["redirect_uri"];
            if (string.IsNullOrEmpty(redirect_uri))
            {
                var authorizeActionData = provider.Actions.GetSection("authorize");
                if (authorizeActionData != null)
                    redirect_uri = authorizeActionData["redirect_uri"];

                if (string.IsNullOrEmpty(redirect_uri))
                    throw new HttpException(400, $"No redirect_uri URL in action.");
            }

            if (string.IsNullOrEmpty(code))
                throw new HttpException(400, $"No code provided in data.");

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

        public static async Task<HttpResponseMessage> Get(IConfigurationSection actionData, string accessToken)
        {
            var userInfoUrl = actionData["url"];
            if (string.IsNullOrEmpty(userInfoUrl))
                throw new HttpException(400, $"No user info URL in action.");

            var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, userInfoUrl);
            userInfoRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var client = new HttpClient();
            var response = await client.SendAsync(userInfoRequest);

            return response;
        }

        public static async Task<T?> Get<T>(IConfigurationSection actionData, string accessToken)
        {
            var response = await Get(actionData, accessToken);
            var body = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<T>(body);

            return data;
        }

        public static async Task<UserInfo?> GetUserInfo(Provider provider, string accessToken)
        {
            var actionData = provider.Actions?.GetSection("userInfo");
            if (!actionData.Exists())
                throw new HttpException(404, $"Action 'userInfo' not found in provider '{provider.Name}'.");

            return await Get<UserInfo>(actionData, accessToken);
        }

        public async Task<object?> CallbackAuthorizeAsync(Provider provider, DataDictionary? data)
        {
            if (provider.Actions == null)
                throw new ArgumentNullException(nameof(provider.Actions), "Provider actions is null.");

            var accessToken = await GetToken(provider, data?.GetString("code") ?? "");
            if (string.IsNullOrEmpty(accessToken))
                throw new HttpException(400, $"No access token received.");

            var userInfo = await GetUserInfo(provider, accessToken)
                ?? throw new HttpException(400, $"No user info.");

            var eventBus = serviceProvider.GetRequiredService<IEventBus>();
            if (eventBus == null)
                return null;

            var evtData = new DataDictionary {
                { "Username", userInfo.Username ?? userInfo.Email ?? userInfo.Name },
                { "FullName", userInfo.FullName ?? userInfo.Name ?? userInfo.Username ?? userInfo.Email },
                { "Email", userInfo.Email },
                { "DeviceToken", data?.GetString("deviceToken") },
            };

            var tokenActionData = provider.Actions.GetSection("token");
            if (tokenActionData.Exists())
            {
                var selfServiceRegistration = tokenActionData.GetValue<bool?>("selfServiceRegistration");
                if (selfServiceRegistration != null)
                {
                    evtData["SelfServiceRegistration"] = selfServiceRegistration;
                }

                var mandatoryRoles = tokenActionData.GetValue<bool?>("mandatoryRoles");
                if (mandatoryRoles != null)
                {
                    evtData["MandatoryRoles"] = mandatoryRoles;
                }

                List<string> roles = [];
                var rolesFrom = tokenActionData.GetSection("rolesFrom");
                if (rolesFrom.Exists())
                {
                    List<string>? rolesFromAccessTokenList;
                    var rolesFromAccessTokenSection = rolesFrom.GetSection("access_token");
                    if (rolesFromAccessTokenSection.GetChildren().Any())
                        rolesFromAccessTokenList = rolesFromAccessTokenSection.Get<List<string>>();
                    else
                        rolesFromAccessTokenList = [rolesFromAccessTokenSection.Value];

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
            await eventBus.FireAsync("login", evtOptions);

            return evtOptions["Response"];
        }
    }
}
