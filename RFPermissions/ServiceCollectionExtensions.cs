using Microsoft.Extensions.DependencyInjection;
using RFPermissions.IServices;
using RFPermissions.Services;

namespace RFPermissionsServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFPermissionsServices(this IServiceCollection services)
        {
            services.AddScoped<IPermissionService, PermissionService>();

            return services;
        }

    }
}
