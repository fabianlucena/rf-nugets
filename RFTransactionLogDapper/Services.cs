using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFService.IRepo;
using RFTransactionLog.Entities;

namespace RFTransactionLogDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFTransactionLogDapper(this IServiceCollection services)
        {
            services.AddScoped<Dapper<LogLevel>, Dapper<LogLevel>>();
            services.AddScoped<Dapper<LogAction>, Dapper<LogAction>>();
            services.AddScoped<Dapper<TransactionLog>, Dapper<TransactionLog>>();

            services.AddScoped<IRepo<LogLevel>, Dapper<LogLevel>>();
            services.AddScoped<IRepo<LogAction>, Dapper<LogAction>>();
            services.AddScoped<IRepo<TransactionLog>, Dapper<TransactionLog>>();
        }
    }
}
