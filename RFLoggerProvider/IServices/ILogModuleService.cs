using RFLoggerProvider.Entities;
using RFService.IServices;

namespace RFLoggerProvider.IServices
{
    public interface ILogModuleService
        : IService<LogModule>,
            IServiceId<LogModule>,
            IServiceUuid<LogModule>,
            IServiceName<LogModule>,
            IServiceIdUuidName<LogModule>
    {
    }
}
