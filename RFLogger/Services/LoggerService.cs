using RFLogger.IServices;
using RFLogger.Types;

namespace RFLogger.Services
{
    public class LoggerService
        : ILoggerService
    {
        private readonly List<LoggerProvider> Providers = [];

        public void AddProvider(LoggerProvider provider)
            => Providers.Add(provider);

        public async Task<IEnumerable<object?>> AddAsync(LLevel level, LAction action, string message, object? data = null, IDictionary<string, object>? options = null)
        {
            List<object?> result = [];
            foreach (var provider in Providers)
                result.Add(await provider(level, action, message, data, options));

            return result;
        }

        public Task<IEnumerable<object?>> AddInfoAsync(LAction action, string message, object? data = null, IDictionary<string, object>? options = null)
            => AddAsync(LLevel.INFO, action, message, data, options);

        public Task<IEnumerable<object?>> AddInfoGetAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoAsync(LAction.GET, message, data, options);

        public Task<IEnumerable<object?>> AddInfoAddAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoAsync(LAction.ADD, message, data, options);

        public Task<IEnumerable<object?>> AddInfoEditAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoAsync(LAction.EDIT, message, data, options);

        public Task<IEnumerable<object?>> AddInfoDeleteAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoAsync(LAction.DELETE, message, data, options);

        public Task<IEnumerable<object?>> AddInfoRestoreAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddInfoAsync(LAction.RESTORE, message, data, options);

        public Task<IEnumerable<object?>> AddErrorAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddAsync(LLevel.ERROR, LAction.NONE, message, data, options);
        
        public Task<IEnumerable<object?>> AddExceptionAsync(string message, object? data = null, IDictionary<string, object>? options = null)
            => AddAsync(LLevel.EXCEPTION, LAction.NONE, message, data, options);
    }
}
