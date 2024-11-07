using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFService.IRepo;
using RFUserEmail.Entities;

namespace RFUserEmailDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFUserEmailDapper(this IServiceCollection services)
        {
            services.AddScoped<Dapper<UserEmail>, Dapper<UserEmail>>();

            services.AddScoped<IRepo<UserEmail>, Dapper<UserEmail>>();
        }
    }
}
