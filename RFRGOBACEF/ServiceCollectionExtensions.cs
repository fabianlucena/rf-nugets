using Microsoft.Extensions.DependencyInjection;
using RFRBACEF.Repositories;
using RFRBACIRepositories.IRepositories;
using RFRGOBACEF.Repositories;
using RFRGOBACIRepositories.IRepositories;

namespace RFRGOBACEF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFRGOBACEF(this IServiceCollection services)
        {
            services.AddScoped<ISessionOrganizationRepository, SessionOrganizationRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleIncludeRepository, RoleIncludeRepository>();
            services.AddScoped<IRoleXUserXOrganizationRepository, RoleXUserXOrganizationRepository>();
            services.AddScoped<IPermissionXRoleRepository, PermissionXRoleRepository>();

            return services;
        }

    }
}
