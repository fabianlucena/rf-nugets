namespace RFAuth.DTO
{
    public class LoginResponse
    {
        public string AuthorizationToken { get; set; } = string.Empty;

        public string AutoLoginToken { get; set; } = string.Empty;

        public string DeviceToken { get; set; } = string.Empty;

        public IDictionary<string, object>? Attributes { get; set; }
    }
}
