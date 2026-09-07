using RFLoggerProvider.Entities;
using RFLoggerProvider.IRepositories;
using RFLoggerProvider.IServices;
using RFServices.Services;

namespace RFLoggerProvider.Services;

public class LogActionService(
    ILogActionRepository logActionRepository,
    IServiceProvider serviceProvider
)
    : NominableEntityService<LogAction>(logActionRepository, serviceProvider),
    ILogActionService
{
}
