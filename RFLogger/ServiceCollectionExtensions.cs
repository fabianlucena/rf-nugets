using Microsoft.Extensions.DependencyInjection;
using RFLogger.IServices;
using RFLogger.Services;

namespace RFLogger
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFLoggerServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerService>();

            return services;
        }
    }
}