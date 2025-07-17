using RFService.Libs;

namespace RFAuth.DTO
{
    public class LoginData
    {
        public Int64 UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string AuthorizationToken { get; set; } = string.Empty;
        public string AutoLoginToken { get; set; } = string.Empty;
        public string DeviceToken { get; set; } = string.Empty;
        public DataDictionary? Attributes { get; set; }
    }
}
