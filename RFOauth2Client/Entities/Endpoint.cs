namespace RFOauth2Client.Entities
{
    public class Endpoint
    {
        public string URL { get; set; } = string.Empty;
        public string? Method { get; set; }
        public Dictionary<string, string>? Parameters { get; set; }
        public bool? IncludeClientId { get; set; }
        public bool? IncludeRedirectUri { get; set; }

        public Dictionary<string, string> GetParameters(Provider provider, Dictionary<string, string>? defaultValues = null)
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
            
            if (Parameters is not null)
            {
                foreach (var param in Parameters)
                    parameters[param.Key] = param.Value;
            }

            return parameters;
        }

        public string GetFullURL(Provider provider, Dictionary<string, string>? defaultValues = null)
        {
            var fullURL = provider.Client.URLBase + URL;

            var parameters = GetParameters(provider, defaultValues);
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
