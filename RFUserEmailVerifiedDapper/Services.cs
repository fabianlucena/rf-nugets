using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFService.IRepo;
using RFUserEmailVerified.Entities;

namespace RFUserEmailVerifiedDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFUserEmailVerifiedDapper(this IServiceCollection services)
        {
            services.AddScoped<Dapper<UserEmailVerified>, Dapper<UserEmailVerified>>();

            services.AddScoped<IRepo<UserEmailVerified>, Dapper<UserEmailVerified>>();
        }
    }
}
