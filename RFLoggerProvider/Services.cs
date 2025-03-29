using Microsoft.Extensions.DependencyInjection;
using RFLoggerProvider.IServices;
using RFLoggerProvider.Services;

namespace RFLoggerProvider
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFLoggerProvider(this IServiceCollection services)
        {
            services.AddScoped<ILogLevelService, LogLevelService>();
            services.AddScoped<ILogActionService, LogActionService>();
            services.AddScoped<ILogService, LogService>();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
    }
}