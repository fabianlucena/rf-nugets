using Microsoft.Extensions.DependencyInjection;
using RFPermissions.IServices;
using RFPermissions.Services;

namespace RFPermissions;

public static class AddRFPermissionsServicesRegistration
{
    public static IServiceCollection AddRFPermissionsServices(this IServiceCollection services)
    {
        services.AddScoped<IPermissionService, PermissionService>();

        return services;
    }

}
