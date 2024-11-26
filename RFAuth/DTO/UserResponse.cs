namespace RFAuth.DTO
{
    public class UserResponse
    {
        public Guid Uuid { get; set; }

        public bool IsEnabled { get; set; }

        public required string Username { get; set; }

        public required string FullName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public IDictionary<string, object>? Attributes { get; set; }
    }
}
