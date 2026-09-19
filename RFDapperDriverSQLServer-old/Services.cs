using Microsoft.Extensions.DependencyInjection;
using RFDapper;

namespace RFDapperDriverSQLServer
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFDapperDriverSQLServer(this IServiceCollection services, SQLServerDDOptions options)
        {
            services.AddScoped<IDriver>(provider => new SQLServerDD(options));
        }
    }
}
