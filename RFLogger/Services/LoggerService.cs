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

        public async Task<IEnumerable<object>> AddAsync(LLevel level, LAction action, string message, object? data = null)
        {
            List<object> result = [];
            foreach (var provider in Providers)
                result.Add(await provider(level, action, message, data));

            return result;
        }

        public Task<IEnumerable<object>> AddInfoAsync(LAction action, string message, object? data = null)
            => AddAsync(LLevel.INFO, action, message, data);

        public Task<IEnumerable<object>> AddInfoGetAsync(string message, object? data = null)
            => AddInfoAsync(LAction.GET, message, data);
    }
}
