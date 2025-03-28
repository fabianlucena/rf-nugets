using RFService.IServices;
using RFTransactionLog.Entities;

namespace RFTransactionLog.IServices
{
    public interface ITransactionLogService
        : IService<TransactionLog>,
            IServiceId<TransactionLog>,
            IServiceUuid<TransactionLog>,
            IServiceIdUuid<TransactionLog>
    {
        Task<TransactionLog> AddAsync(Int64 levelId, Int64 actionId, string message, object? data = null, bool? dataRequest = null);

        Task<TransactionLog> AddAsync(TLLevel level, TLAction action, string message, object? data = null, bool? dataRequest = null);

        Task<TransactionLog> AddInfoAsync(TLAction actionId, string message, object? data = null, bool? dataRequest = null);
    }
}
