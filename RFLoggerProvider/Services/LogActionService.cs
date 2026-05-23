using RFLoggerProvider.Entities;
using RFLoggerProvider.IRepositories;
using RFLoggerProvider.IServices;
using RFServices.Services;

namespace RFLoggerProvider.Services
{
    public class LogActionService(ILogActionRepository logActionRepository)
        : NominableEntityService<LogAction>(logActionRepository),
        ILogActionService
    {
    }
}
