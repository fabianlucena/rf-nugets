namespace RFIServices.DTO
{
    public class UserAddRequest
    {
        public required string Username { get; set; }

        public required string Password { get; set; }

        public required string DisplayName { get; set; }
    }
}
