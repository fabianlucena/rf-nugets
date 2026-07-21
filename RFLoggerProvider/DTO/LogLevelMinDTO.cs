using RFLoggerProvider.Entities;

namespace RFLoggerProvider.DTO
{
    public class LogLevelMinDTO(LogLevel entity)
    {
        public Guid Uuid { get; } = entity.Uuid;
        public string Name { get; } = entity.Name;
    }
}
