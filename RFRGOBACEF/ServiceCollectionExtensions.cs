using Microsoft.Extensions.DependencyInjection;
using RFRBAC.IRepositories;
using RFRBACEF.Repositories;
using RFRGOBAC.IRepositories;
using RFRGOBACEF.Repositories;

namespace RFRGOBACEF;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRFRGOBACEF(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<ISessionOrganizationRepository, SessionOrganizationRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleIncludeRepository, RoleIncludeRepository>();
        services.AddScoped<IRoleXUserXOrganizationRepository, RoleXUserXOrganizationRepository>();
        services.AddScoped<IPermissionXRoleRepository, PermissionXRoleRepository>();

        return services;
    }

}
