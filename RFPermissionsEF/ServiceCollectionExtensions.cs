using Microsoft.Extensions.DependencyInjection;
using RFPermissions.IRepositories;
using RFPermissionsEF.Repositories;

namespace RFPermissionsEF;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRFPermissionsEF(this IServiceCollection services)
    {
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        return services;
    }
}
