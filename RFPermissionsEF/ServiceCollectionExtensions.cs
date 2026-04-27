using Microsoft.Extensions.DependencyInjection;
using RFPermissionsEF.Repositories;
using RFPermissionsIRepositories.Repositories;

namespace RFPermissionsEF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFPermissionsEF(this IServiceCollection services)
        {
            services.AddScoped<IPermissionRepository, PermissionRepository>();

            return services;
        }

    }
}
