using RFAuth.IServices;
using RFLogger.Services;
using RFLogger.Types;
using RFRegisterService.Attributes;

namespace RFAuth.Services;

[RegisterService]

public class RFAuthLoggerService : LoggerService, IRFAuthLoggerService
{
    public async Task<IEnumerable<object?>> AddInfoGetAsync(string message, object? data = null, IDictionary<string, object>? options = null)
        => await AddInfoAsync("RFAuth", LAction.GET, message, data, options);

    public async Task<IEnumerable<object?>> AddInfoAddAsync(string message, object? data = null, IDictionary<string, object>? options = null)
        => await AddInfoAsync("RFAuth", LAction.ADD, message, data, options);

    public async Task<IEnumerable<object?>> AddInfoEditAsync(string message, object? data = null, IDictionary<string, object>? options = null)
        => await AddInfoAsync("RFAuth", LAction.EDIT, message, data, options);

    public async Task<IEnumerable<object?>> AddInfoDeleteAsync(string message, object? data = null, IDictionary<string, object>? options = null)
        => await AddInfoAsync("RFAuth", LAction.DELETE, message, data, options);
}
