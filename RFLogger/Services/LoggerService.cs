using RFLogger.IServices;
using RFLogger.Types;

namespace RFLogger.Services
{
    public class LoggerService
        : ILoggerService
    {
        private static readonly List<LoggerProvider> Providers = [];

        public void AddProvider(LoggerProvider provider)
            => Providers.Add(provider);

        public async Task<IEnumerable<object?>> AddAsync(LLevel level, string module, LAction action, string message, object? data = null, IDictionary<string, object>? options = null)
        {
            List<object?> result = [];
            foreach (var provider in Providers)
                result.Add(await provider(level, module, action, message, data, options));

            return result;
        }

        public Task<IEnumerable<object?>> AddInfoAsync(string module, LAction action, string message, object? data = null, IDictionary<string, object>? options = null)
            => AddAsync(LLevel.INFO, module, action, message, data, options);

        public virtual Task<IEnumerable<object?>> AddInfoGetAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoAsync(module, LAction.GET, message, data, options);

        public Task<IEnumerable<object?>> AddInfoAddAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoAsync(module, LAction.ADD, message, data, options);

        public Task<IEnumerable<object?>> AddInfoEditAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoAsync(module, LAction.EDIT, message, data, options);

        public Task<IEnumerable<object?>> AddInfoDeleteAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoAsync(module, LAction.DELETE, message, data, options);

        public Task<IEnumerable<object?>> AddInfoRestoreAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoAsync(module, LAction.RESTORE, message, data, options);

        public Task<IEnumerable<object?>> AddErrorAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null)
            => AddAsync(LLevel.ERROR, module, LAction.NONE, message, data, options);
        
        public Task<IEnumerable<object?>> AddExceptionAsync(string module, string message, object? data = null, IDictionary<string, object>? options = null)
            => AddAsync(LLevel.EXCEPTION, module, LAction.NONE, message, data, options);
    }
}
