using RFLogger.Services;
using RFLogger.Types;
using RFRegisterService.Attributes;
using RFRGOBAC.IServices;

namespace RFRGOBAC.Services;

[RegisterService]
public class RFRGOBACLoggerService : LoggerService, IRFRGOBACLoggerService
{
    public async Task<IEnumerable<object?>> AddInfoGetAsync(string message, object? data = null, IDictionary<string, object>? options = null)
        => await AddInfoAsync("RFRGOBAC", LAction.GET, message, data, options);

    public async Task<IEnumerable<object?>> AddInfoAddAsync(string message, object? data = null, IDictionary<string, object>? options = null)
        => await AddInfoAsync("RFRGOBAC", LAction.ADD, message, data, options);

    public async Task<IEnumerable<object?>> AddInfoEditAsync(string message, object? data = null, IDictionary<string, object>? options = null)
        => await AddInfoAsync("RFRGOBAC", LAction.EDIT, message, data, options);

    public async Task<IEnumerable<object?>> AddInfoDeleteAsync(string message, object? data = null, IDictionary<string, object>? options = null)
        => await AddInfoAsync("RFRGOBAC", LAction.DELETE, message, data, options);
}
