namespace RFHttpAction.DTO
{
    public class HttpActionResponse
    {
        public required Guid Uuid { get; set; }

        public HttpActionTypeDTO? Type { get; set; }

        public required string Token { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required DateTime UpdatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public object? Data { get; set; }
    }
}
