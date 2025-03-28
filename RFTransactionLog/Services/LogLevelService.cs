using RFService.Services;
using RFService.IRepo;
using RFTransactionLog.Entities;
using RFTransactionLog.IServices;

namespace RFTransactionLog.Services
{
    public class LogLevelService(IRepo<LogLevel> repo)
        : ServiceIdUuidName<IRepo<LogLevel>, LogLevel>(repo),
            ILogLevelService
    {
    }
}
