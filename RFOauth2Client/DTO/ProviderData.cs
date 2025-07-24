using Microsoft.Extensions.Configuration;

namespace RFOauth2Client.DTO
{
    public class ProviderData
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public IConfigurationSection? Actions { get; set; }
    }
}
