using Microsoft.Extensions.DependencyInjection;
using RFDapper;

namespace RFDapperDriverMySQL
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFDapperDriverMySQL(this IServiceCollection services, MySQLDDOptions options)
        {
            services.AddScoped<IDriver>(provider => new MySQLDD(options));
        }
    }
}
