using RFLogger.Services;
using RFLogger.Types;
using RFRBAC.IServices;
using RFServices.Attributes;

namespace RFRBAC.Services;

[RegisterService]

public class RFRBACLoggerService : LoggerService, IRFRBACLoggerService
{
    public async Task<IEnumerable<object?>> AddInfoGetAsync(string message, object? data = null, IDictionary<string, object>? options = null)
        => await AddInfoAsync("RFRBAC", LAction.GET, message, data, options);
}
