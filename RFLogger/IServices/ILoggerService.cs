using RFLogger.Types;

namespace RFLogger.IServices
{
    public interface ILoggerService
    {
        void AddProvider(LoggerProvider provider);

        Task<IEnumerable<object?>> AddAsync(LLevel level, LAction action, string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoAsync(LAction action, string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoGetAsync(string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoAddAsync(string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoEditAsync(string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoDeleteAsync(string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoRestoreAsync(string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddErrorAsync(string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddExceptionAsync(string message, object? data = null, IDictionary<string, object>? options = null);
    }
}
