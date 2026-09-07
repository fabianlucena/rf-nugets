using RFLoggerProvider.Entities;
using RFLoggerProvider.IRepositories;
using RFLoggerProvider.IServices;
using RFServices.Services;

namespace RFLoggerProvider.Services;

public class LogLevelService(
    ILogLevelRepository logLevelRepository,
    IServiceProvider serviceProvider
)
    : NominableEntityService<LogLevel>(logLevelRepository, serviceProvider),
    ILogLevelService
{
}
