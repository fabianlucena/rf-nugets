using RFLogger.IServices;

namespace RFRGOBAC.IServices;

public interface IRFRGOBACLoggerService
    : ILoggerService
{
    Task<IEnumerable<object?>> AddInfoGetAsync(string message, object? data = null, IDictionary<string, object>? options = null);

    Task<IEnumerable<object?>> AddInfoAddAsync(string message, object? data = null, IDictionary<string, object>? options = null);

    Task<IEnumerable<object?>> AddInfoEditAsync(string message, object? data = null, IDictionary<string, object>? options = null);

    Task<IEnumerable<object?>> AddInfoDeleteAsync(string message, object? data = null, IDictionary<string, object>? options = null);
}
