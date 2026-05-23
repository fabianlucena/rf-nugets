using RFLoggerProvider.Entities;
using RFLoggerProvider.IRepositories;
using RFLoggerProvider.IServices;
using RFServices.Services;

namespace RFLoggerProvider.Services
{
    public class LogModuleService(ILogModuleRepository logModuleRepository)
        : NominableEntityService<LogModule>(logModuleRepository),
        ILogModuleService
    {
    }
}
