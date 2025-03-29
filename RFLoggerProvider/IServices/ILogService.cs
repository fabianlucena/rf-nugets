using RFLogger.Types;
using RFLoggerProvider.Entities;
using RFService.IServices;

namespace RFLoggerProvider.IServices
{
    public interface ILogService
        : IService<Log>,
            IServiceId<Log>,
            IServiceUuid<Log>,
            IServiceIdUuid<Log>
    {
        Task<Log> AddAsync(Int64 levelId, Int64 actionId, string message, object? data = null, bool? dataRequest = null);

        Task<Log> AddAsync(LLevel level, LAction action, string message, object? data = null, bool? dataRequest = null);

        Task<Log> AddInfoAsync(LAction action, string message, object? data = null, bool? dataRequest = null);
    }
}
