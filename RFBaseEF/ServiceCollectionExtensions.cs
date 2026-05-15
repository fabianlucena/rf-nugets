using Microsoft.Extensions.DependencyInjection;
using RFBaseEF.Repositories;
using RFBaseIRepositories.IRepositories;

namespace RFBaseEF
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
