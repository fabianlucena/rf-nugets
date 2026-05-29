using Microsoft.Extensions.DependencyInjection;
using RFEntitiesEF.Repositories;
using RFIRepositories.IRepositories;

namespace RFEntitiesEF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFBaseEF(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
