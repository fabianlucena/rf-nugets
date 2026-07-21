using RFAuth.DTO;
using RFIServices.DTO;

namespace RFLoggerProvider.DTO
{
    public class LogDTO
    {
        public Guid Uuid { get; set; }

        public DateTime LogTimestamp { get; set; }

        public required LogLevelMinDTO Level { get; set; }

        public required LogActionMinDTO Action { get; set; }

        public required string Message { get; set; }

        public required string? JsonData { get; set; }

        public SessionMinDTO? Session { get; set; }

        public UserMinDTO? User { get; set; }
    }
}
