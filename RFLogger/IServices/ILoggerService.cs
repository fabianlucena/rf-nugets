using RFLogger.Types;

namespace RFLogger.IServices
{
    public interface ILoggerService
    {
        void AddProvider(LoggerProvider provider);

        Task<IEnumerable<object?>> AddAsync(LLevel level, string module, LAction action, string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoAsync(string module, LAction action, string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoGetAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoAddAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoEditAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoDeleteAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoRestoreAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddErrorAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddExceptionAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null);
    }
}
