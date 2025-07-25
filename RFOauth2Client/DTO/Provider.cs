using RFService.Libs;

namespace RFOauth2Client.DTO
{
    public class Provider
    {
        public string Name { get; set; } = string.Empty;
        public bool Disabled { get; set; } = false;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public DataDictionary Actions { get; set; } = [];
    }
}
