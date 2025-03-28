using RFService.IServices;
using RFTransactionLog.Entities;

namespace RFTransactionLog.IServices
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
