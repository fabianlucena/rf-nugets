using System.Net;

namespace RFOauth2Client.Entities
{
    public class LoginProviderResponse
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string URL { get; set; }

        public LoginProviderResponse(Provider provider)
        {
            var autorizeEndpoint = provider.Endpoints.GetValueOrDefault("authorize")
                ?? throw new Exception($"Provider {provider.Name} does not have an authorize endpoint.");

            Name = provider.Name;
            DisplayName = string.IsNullOrEmpty(provider.DisplayName) ? $"Login with {provider.Name}" : provider.DisplayName;
            URL = autorizeEndpoint.GetFullURL(provider, new Dictionary<string, string>{ { "response_type", "code" } });
        }
    }
}
