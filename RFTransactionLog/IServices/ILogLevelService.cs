using RFService.IServices;
using RFTransactionLog.Entities;

namespace RFTransactionLog.IServices
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
