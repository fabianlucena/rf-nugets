namespace RFOauth2Client.DTO
{
    public class AuthorizeProvider
    {
        public string Label { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ResponseType { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
