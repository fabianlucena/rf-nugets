using RFIServices.IServices;
using RFLogger.Types;
using RFLoggerProvider.Entities;

namespace RFLoggerProvider.IServices
{
    public interface ILogService : ICommonEntityService<Log>
    {
        Task<Log> AddAsync(long levelId, long actionId, string message, object? data = null, bool? dataRequest = null);

        Task<Log> AddAsync(LLevel level, LAction action, string message, object? data = null, bool? dataRequest = null);

        Task<Log> AddInfoAsync(LAction action, string message, object? data = null, bool? dataRequest = null);
    }
}
