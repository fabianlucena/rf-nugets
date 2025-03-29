using Microsoft.Extensions.DependencyInjection;
using RFLogger.IServices;
using RFLogger.Services;

namespace RFLogger
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFLogger(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerService>();
        }
    }
}