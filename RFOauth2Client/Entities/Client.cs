namespace RFOauth2Client.Entities
{
    public class Client
    {
        public string URLBase { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public bool UsePKCE { get; set; } = false;
    }
}
