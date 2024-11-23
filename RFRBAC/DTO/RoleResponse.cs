namespace RFRBAC.DTO
{
    public class RoleResponse
    {
        public Guid Uuid { get; set; }

        public bool IsEnabled { get; set; }

        public required string Name { get; set; }

        public required string Title { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public IDictionary<string, object>? Attributes { get; set; }
    }
}