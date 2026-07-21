namespace RFAuth.DTO
{
    public class AutoLoginRequest
    {
        public required string DeviceToken { get; set; }

        public required string AutoLoginToken { get; set; }
    }
}
