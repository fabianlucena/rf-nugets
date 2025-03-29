using RFLogger.Types;

namespace RFLogger.IServices
{
    public interface ILoggerService
    {
        void AddProvider(LoggerProvider provider);

        Task<IEnumerable<object>> AddAsync(LLevel level, LAction action, string message, object? data = null);

        Task<IEnumerable<object>> AddInfoAsync(LAction action, string message, object? data = null);

        Task<IEnumerable<object>> AddInfoGetAsync(string message, object? data = null);
    }
}
