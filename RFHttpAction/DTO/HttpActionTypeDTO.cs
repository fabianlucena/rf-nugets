namespace RFHttpAction.DTO
{
    public class HttpActionTypeDTO
    {
        public required Guid Uuid { get; set; }

        public bool IsEnabled { get; set; }

        public required string Name { get; set; }

        public required string Title { get; set; }

    }
}
