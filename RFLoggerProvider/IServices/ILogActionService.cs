using RFLoggerProvider.Entities;
using RFService.IServices;

namespace RFLoggerProvider.IServices
{
    public interface ILogActionService
        : IService<LogAction>,
            IServiceId<LogAction>,
            IServiceUuid<LogAction>,
            IServiceName<LogAction>,
            IServiceIdUuidName<LogAction>
    {
    }
}
