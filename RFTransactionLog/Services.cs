using Microsoft.Extensions.DependencyInjection;
using RFTransactionLog.IServices;
using RFTransactionLog.Services;

namespace RFTransactionLog
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFTransactionLog(this IServiceCollection services)
        {
            services.AddScoped<ILogLevelService, LogLevelService>();
            services.AddScoped<ILogActionService, LogActionService>();
            services.AddScoped<ITransactionLogService, TransactionLogService>();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
    }
}