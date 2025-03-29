using RFService.Services;
using RFService.IRepo;
using RFLoggerProvider.Entities;
using RFLoggerProvider.IServices;

namespace RFLoggerProvider.Services
{
    public class LogActionService(IRepo<LogAction> repo)
        : ServiceIdUuidName<IRepo<LogAction>, LogAction>(repo),
            ILogActionService
    {
    }
}
