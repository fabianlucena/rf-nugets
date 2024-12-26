using RFService.Libs;

namespace RFAuth.DTO
{
    public class LoginResponse
    {
        public string Username { get; set; } = string.Empty;

        public string AuthorizationToken { get; set; } = string.Empty;

        public string AutoLoginToken { get; set; } = string.Empty;

        public string DeviceToken { get; set; } = string.Empty;

        public DataDictionary? Attributes { get; set; }
    }
}
