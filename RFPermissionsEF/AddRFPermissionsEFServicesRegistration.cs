using Microsoft.Extensions.DependencyInjection;
using RFPermissions.IRepositories;
using RFPermissionsEF.Repositories;

namespace RFPermissionsEF;

public static class AddRFPermissionsEFServicesRegistration
{
    public static IServiceCollection AddRFPermissionsEFServices(this IServiceCollection services)
    {
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        return services;
    }
}
