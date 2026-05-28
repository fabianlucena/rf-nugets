using RFLogger.IServices;

namespace RFAuth.IServices
{
    public interface IRFAuthLoggerService
        : ILoggerService
    {
        Task<IEnumerable<object?>> AddInfoGetAsync(string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoAddAsync(string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoEditAsync(string message, object? data = null, IDictionary<string, object>? options = null);

        Task<IEnumerable<object?>> AddInfoDeleteAsync(string message, object? data = null, IDictionary<string, object>? options = null);
    }
}
