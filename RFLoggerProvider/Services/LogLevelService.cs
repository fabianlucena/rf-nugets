using RFService.Services;
using RFService.IRepo;
using RFLoggerProvider.Entities;
using RFLoggerProvider.IServices;

namespace RFLoggerProvider.Services
{
    public class LogLevelService(IRepo<LogLevel> repo)
        : ServiceIdUuidName<IRepo<LogLevel>, LogLevel>(repo),
            ILogLevelService
    {
    }
}
