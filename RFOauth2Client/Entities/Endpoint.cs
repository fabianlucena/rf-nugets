namespace RFOauth2Client.Entities
{
    public class Endpoint
    {
        public string URL { get; set; } = string.Empty;
        public string? Method { get; set; }
        public Dictionary<string, string>? Parameters { get; set; }
        public bool? IncludeClientId { get; set; }
        public bool? IncludeRedirectUri { get; set; }
        public bool? IncludeClientSecret { get; set; }
        public bool? IncludeRefreshToken { get; set; }

        public Dictionary<string, string> GetParameters(Provider provider, TokenResponse tokenResponse, Dictionary<string, string>? defaultValues = null)
        {
            Dictionary<string, string> parameters = [];

            if (defaultValues is not null)
            {
                foreach (var param in defaultValues)
                    parameters[param.Key] = param.Value;
            }

            if (IncludeClientId is null || IncludeClientId == true)
                parameters["client_id"] = provider.Client.ClientId;
            
            if (IncludeRedirectUri is null || IncludeRedirectUri == true)
                parameters["redirect_uri"] = provider.Client.RedirectUri;

            if (IncludeClientSecret is not null && IncludeClientSecret == true)
                parameters["client_secret"] = provider.Client.ClientSecret;

            if (IncludeRefreshToken is not null && IncludeRefreshToken == true && tokenResponse != null)
                parameters["refresh_token"] = tokenResponse.RefreshToken;

            if (Parameters is not null)
            {
                foreach (var param in Parameters)
                    parameters[param.Key] = param.Value;
            }

            return parameters;
        }

        public string GetFullURL(Provider provider, TokenResponse tokenResponse, Dictionary<string, string>? defaultValues = null)
        {
            var fullURL = provider.Client.URLBase + URL;

            if (Method?.ToUpper() == "POST")
                return fullURL;

            var parameters = GetParameters(provider, tokenResponse, defaultValues);
            if (parameters.Count > 0)
            {
                fullURL += fullURL.Contains('?') ? '&' : '?';
                fullURL += string.Join("&",
                    parameters.Select(kvp =>
                        $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            }

            return fullURL;
        }
    }
}
