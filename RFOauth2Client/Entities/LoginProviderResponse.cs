namespace RFOauth2Client.Entities
{
    public class LoginProviderResponse(AuthorizeProvider authorizeProvider)
    {
        public string Name { get; set; } = authorizeProvider.Name;
        public string URL { get; set; } = authorizeProvider.Url;
        public string Label { get; set; } = authorizeProvider.Label;
    }
}
