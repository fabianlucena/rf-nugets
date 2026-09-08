using RFLoggerProvider.Entities;

namespace RFLoggerProvider.DTO;

public class LogResponse(Log log)
    : LogDTO(log)
{
}
