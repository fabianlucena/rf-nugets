using Microsoft.Extensions.DependencyInjection;
using RFRBACEF.Repositories;
using RFRBACIRepositories.IRepositories;

namespace RFRBACEF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFRBACEF(this IServiceCollection services)
        {
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleIncludeRepository, RoleIncludeRepository>();
            services.AddScoped<IRoleXUserRepository, RoleXUserRepository>();
            services.AddScoped<IPermissionXRoleRepository, PermissionXRoleRepository>();

            return services;
        }

    }
}
