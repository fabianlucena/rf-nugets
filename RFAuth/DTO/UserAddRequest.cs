namespace RFAuth.DTO
{
    public class UserAddRequest
    {
        public required string Username { get; set; }

        public required string Password { get; set; }

        public required string FullName { get; set; }
    }
}
