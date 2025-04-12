using RFAuth.IServices;
using RFLogger.Services;

namespace RFAuth.Services
{
    public class RFAuthLoggerService
        : LoggerService,
            IRFAuthLoggerService
    {
        public Task<IEnumerable<object?>> AddInfoGetAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoGetAsync("RFAuth", message, data, options);

        public Task<IEnumerable<object?>> AddInfoAddAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoAddAsync("RFAuth", message, data, options);

        public Task<IEnumerable<object?>> AddInfoEditAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoEditAsync("RFAuth", message, data, options);

        public Task<IEnumerable<object?>> AddInfoDeleteAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoDeleteAsync("RFAuth", message, data, options);
    }
}
