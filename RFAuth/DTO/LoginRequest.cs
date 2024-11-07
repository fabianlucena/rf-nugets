namespace RFAuth.DTO
{
    public class LoginRequest
    {
        public required string Username { get; set; }

        public required string Password { get; set; }

        public string? DeviceToken { get; set; }
    }
}
