using RFLogger.IServices;

namespace RFRBAC.IServices
{
    public interface IRFRBACLoggerService
        : ILoggerService
    {
        Task<IEnumerable<object?>> AddInfoGetAsync(string message, object? data = null, IDictionary<string, object>? options = null);
    }
}
