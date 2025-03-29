using RFLoggerProvider.Entities;
using RFService.IServices;

namespace RFLoggerProvider.IServices
{
    public interface ILogLevelService
        : IService<LogLevel>,
            IServiceId<LogLevel>,
            IServiceUuid<LogLevel>,
            IServiceName<LogLevel>,
            IServiceIdUuidName<LogLevel>
    {
    }
}
