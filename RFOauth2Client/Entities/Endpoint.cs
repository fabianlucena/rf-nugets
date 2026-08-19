using RFOauth2Client.Exceptions;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RFOauth2Client.Entities;

public class Endpoint
{
    public string Name { get; set; } = string.Empty;
    public string URL { get; set; } = string.Empty;
    public Method? Method { get; set; }
    public bool? AuthorizationHeader { get; set; }

    public Dictionary<string, string>? Query { get; set; }
    public bool? ClientIdInQuery { get; set; }
    public bool? RedirectUriInQuery { get; set; }
    public bool? ClientSecretInQuery { get; set; }
    public bool? RefreshTokenInQuery { get; set; }

    [JsonPropertyName("contentType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ContentType? ContentType { get; set; }
    public Dictionary<string, string>? Body { get; set; }
    public bool? ClientIdInBody{ get; set; }
    public bool? RedirectUriInBody { get; set; }
    public bool? ClientSecretInBody { get; set; }
    public bool? RefreshTokenInBody { get; set; }

    public void AddQueryParameter(string key, string value)
    {
        Query ??= [];
        Query[key] = value;
    }

    public void AddBodyParameter(string key, string value)
    {
        Body ??= [];
        Body[key] = value;
    }

    public Dictionary<string, string> GetQueryParameters(Provider provider, TokenResponse? tokenResponse, Dictionary<string, string>? defaultValues = null)
    {
        Dictionary<string, string> parameters = [];

        if (defaultValues is not null)
        {
            foreach (var param in defaultValues)
                parameters[param.Key] = param.Value;
        }

        if (ClientIdInQuery == true)
            parameters["client_id"] = provider.Client.ClientId;
        
        if (RedirectUriInQuery == true)
            parameters["redirect_uri"] = provider.Client.RedirectUri;

        if (ClientSecretInQuery == true)
            parameters["client_secret"] = provider.Client.ClientSecret;

        if (RefreshTokenInQuery == true)
        {
            if (tokenResponse == null)
                throw new NoTokenResponseException();

            if (string.IsNullOrEmpty(tokenResponse.RefreshToken))
                throw new NoRefreshTokenException();

            parameters["refresh_token"] = tokenResponse.RefreshToken;
        }

        if (Query is not null)
        {
            foreach (var param in Query)
                parameters[param.Key] = param.Value;
        }

        return parameters;
    }

    public Dictionary<string, string> GetBodyParameters(Provider provider, TokenResponse? tokenResponse, Dictionary<string, string>? defaultValues = null)
    {
        Dictionary<string, string> parameters = [];

        if (defaultValues is not null)
        {
            foreach (var param in defaultValues)
                parameters[param.Key] = param.Value;
        }

        if (ClientIdInBody == true)
        {
            if (string.IsNullOrEmpty(provider.Client.ClientId))
                throw new NoClientIdException(Name);

            parameters["client_id"] = provider.Client.ClientId;
        }

        if (RedirectUriInBody == true)
        {
            if (string.IsNullOrEmpty(provider.Client.RedirectUri))
                throw new NoRedirectURIException(Name);

            parameters["redirect_uri"] = provider.Client.RedirectUri;
        }

        if (ClientSecretInBody == true)
        {
            if (string.IsNullOrEmpty(provider.Client.ClientSecret))
                throw new NoClientSecretException(Name);

            parameters["client_secret"] = provider.Client.ClientSecret;
        }

        if (RefreshTokenInBody == true)
        {
            if (tokenResponse == null)
                throw new NoTokenResponseException();

            if (string.IsNullOrEmpty(tokenResponse.RefreshToken))
                throw new NoRefreshTokenException();

            parameters["refresh_token"] = tokenResponse.RefreshToken;
        }

        if (Body is not null)
        {
            foreach (var param in Body)
                parameters[param.Key] = param.Value;
        }

        return parameters;
    }

    public string GetFullURL(Provider provider, TokenResponse? tokenResponse, Dictionary<string, string>? defaultValues = null)
    {
        var fullURL = provider.Client.URLBase + URL;

        if (string.IsNullOrEmpty(fullURL))
            throw new NoURLException(Name);

        var parameters = GetQueryParameters(provider, tokenResponse, defaultValues);
        if (parameters.Count > 0)
        {
            fullURL += fullURL.Contains('?') ? '&' : '?';
            fullURL += string.Join("&",
                parameters.Select(kvp =>
                    $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
        }

        return fullURL;
    }

    public async Task<HttpResponseMessage> Request(Provider provider, TokenResponse? tokenResponse)
    {
        var url = GetFullURL(provider, tokenResponse);
        if (string.IsNullOrEmpty(url))
            throw new NoUserInfoInActionException();

        var method = HttpMethod.Parse(Method?.ToString() ?? "GET");

        var request = new HttpRequestMessage(method, url);
        if (AuthorizationHeader != false)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse?.AccessToken);

        if (method == HttpMethod.Post || method == HttpMethod.Put)
        {
            if (ContentType == Entities.ContentType.Json)
            {
                var bodyParameters = GetBodyParameters(provider, tokenResponse);
                var jsonContent = JsonSerializer.Serialize(bodyParameters);
                request.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            }
            else if (ContentType == Entities.ContentType.FormUrlEncoded)
            {
                request.Content = new FormUrlEncodedContent(GetBodyParameters(provider, tokenResponse));
            }
            else
                throw new UnsupportedContentTypeException(Name, ContentType?.ToString() ?? "null");
        }

        var client = new HttpClient();
        var response = await client.SendAsync(request);

        return response;
    }
}
