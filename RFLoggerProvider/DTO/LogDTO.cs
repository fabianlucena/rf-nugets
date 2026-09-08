using RFAuth.DTO;
using RFIServices.DTO;
using RFLoggerProvider.Entities;

namespace RFLoggerProvider.DTO;

public class LogDTO(Log log)
{
    public Guid Uuid { get; set; } = log.Uuid;
    public DateTime LogTimestamp { get; set; } = log.LogTimestamp;
    public LogLevelMinDTO? Level { get; set; } = log.Level is not null ? new LogLevelMinDTO(log.Level) : null;
    public LogActionMinDTO? Action { get; set; } = log.Action is not null ? new LogActionMinDTO(log.Action) : null;
    public string? Message { get; set; } = log.Message;
    public string? JsonData { get; set; } = log.JsonData;
    public SessionMinDTO? Session { get; set; } = log.Session is not null ? new SessionMinDTO(log.Session) : null;
    public UserMinDTO? User { get; set; } = log.Session?.User is not null ? new UserMinDTO(log.Session.User) : null;
}
