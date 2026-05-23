using Microsoft.Extensions.DependencyInjection;
using RFLoggerProvider.IServices;
using RFLoggerProvider.Services;

namespace RFLoggerProviderServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFLoggerProviderServices(this IServiceCollection services)
        {
            services.AddScoped<ILogLevelService, LogLevelService>();
            services.AddScoped<ILogModuleService, LogModuleService>();
            services.AddScoped<ILogActionService, LogActionService>();
            services.AddScoped<ILogService, LogService>();

            return services;
        }
    }
}