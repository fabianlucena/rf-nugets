using RFAuth.DTO;

namespace RFRegister.DTO
{
    public class RegisterRequest : UserAddRequest
    {
        public required string Email { get; set; }

        public string? DeviceToken { get; set; }
    }
}
