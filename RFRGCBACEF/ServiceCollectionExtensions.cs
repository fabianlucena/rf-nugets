using Microsoft.Extensions.DependencyInjection;
using RFRBACEF.Repositories;
using RFRBACIRepositories.IRepositories;
using RFRGCBACEF.Repositories;
using RFRGCBACIRepositories.IRepositories;

namespace RFRGCBACEF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFRGCBACEF(this IServiceCollection services)
        {
            services.AddScoped<ISessionCompanyRepository, SessionCompanyRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleIncludeRepository, RoleIncludeRepository>();
            services.AddScoped<IRoleXUserXCompanyRepository, RoleXUserXCompanyRepository>();
            services.AddScoped<IPermissionXRoleRepository, PermissionXRoleRepository>();

            return services;
        }

    }
}
