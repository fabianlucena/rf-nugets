using RFLoggerProvider.Entities;

namespace RFLoggerProvider.DTO
{
    public class LogActionMinDTO(LogAction entity)
    {
        public Guid Uuid { get; } = entity.Uuid;
        public string Name { get; } = entity.Name;
    }
}
