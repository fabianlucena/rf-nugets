using RFLoggerProvider.Entities;
using RFLoggerProvider.IRepositories;
using RFLoggerProvider.IServices;
using RFServices.Services;

namespace RFLoggerProvider.Services;

public class LogModuleService(
    ILogModuleRepository logModuleRepository,
    IServiceProvider serviceProvider
)
    : NominableEntityService<LogModule>(logModuleRepository, serviceProvider),
    ILogModuleService
{
}
