using RFService.Services;
using RFService.IRepo;
using RFLoggerProvider.Entities;
using RFLoggerProvider.IServices;

namespace RFLoggerProvider.Services
{
    public class LogModuleService(IRepo<LogModule> repo)
        : ServiceIdUuidName<IRepo<LogModule>, LogModule>(repo),
            ILogModuleService
    {
    }
}
