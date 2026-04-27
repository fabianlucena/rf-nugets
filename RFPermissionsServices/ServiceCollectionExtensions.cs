using Microsoft.Extensions.DependencyInjection;
using RFPermissionsIServices.IServices;
using RFPermissionsServices.Services;

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
