using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFLoggerProvider.Entities;
using RFService.IRepo;

namespace RFLoggerProviderDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFLoggerProviderDapper(this IServiceCollection services)
        {
            services.AddScoped<Dapper<LogLevel>, Dapper<LogLevel>>();
            services.AddScoped<Dapper<LogAction>, Dapper<LogAction>>();
            services.AddScoped<Dapper<Log>, Dapper<Log>>();

            services.AddScoped<IRepo<LogLevel>, Dapper<LogLevel>>();
            services.AddScoped<IRepo<LogAction>, Dapper<LogAction>>();
            services.AddScoped<IRepo<Log>, Dapper<Log>>();
        }
    }
}
