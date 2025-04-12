using RFLogger.Services;
using RFRBAC.IServices;

namespace RFRBAC.Services
{
    public class RFRBACLoggerService
        : LoggerService,
            IRFRBACLoggerService
    {
        public Task<IEnumerable<object?>> AddInfoGetAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoGetAsync("RFRBAC", message, data, options);
    }
}
