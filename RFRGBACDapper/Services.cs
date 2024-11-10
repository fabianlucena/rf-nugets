using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFRGBAC.Entities;
using RFService.IRepo;

namespace RFRGBACDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFRGBACDapper(this IServiceCollection services)
        {
            services.AddScoped<Dapper<UserGroup>, Dapper<UserGroup>>();

            services.AddScoped<IRepo<UserGroup>, Dapper<UserGroup>>();
        }
    }
}
