namespace RFIServices.DTO
{
    public class UserCreateRequest
    {
        public required string Username { get; set; }

        public required string Password { get; set; }

        public required string DisplayName { get; set; }
    }
}
