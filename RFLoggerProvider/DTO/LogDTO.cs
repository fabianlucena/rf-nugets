using RFAuth.DTO;

namespace RFLoggerProvider.DTO
{
    public class LogDTO
    {
        public Guid Uuid { get; set; }

        public DateTime LogTimestamp { get; set; }

        public required LogLevelDTO Level { get; set; }

        public required LogActionDTO Action { get; set; }

        public required string Message { get; set; }

        public required string? JsonData { get; set; }

        public SessionDTO? Session { get; set; }

        public UserDTO? User { get; set; }
    }
}
