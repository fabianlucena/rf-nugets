using Microsoft.Extensions.DependencyInjection;
using RFDapper;

namespace RFDapperDriverPostgreSQL
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFDapperDriverPostgreSQL(this IServiceCollection services, PostgreSQLDDOptions options)
        {
            services.AddScoped<IDriver>(provider => new PostgreSQLDD(options));
        }
    }
}
