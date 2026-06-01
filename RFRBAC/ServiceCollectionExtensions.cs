using Microsoft.Extensions.DependencyInjection;
using RFRBAC.IServices;
using RFRBAC.Services;

namespace RFRBAC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFRBACServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IRFRBACLoggerService, RFRBACLoggerService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleIncludeService, RoleIncludeService>();
            services.AddScoped<IRoleXUserService, RoleXUserService>();
            services.AddScoped<IPermissionXRoleService, PermissionXRoleService>();

            return services;
        }

    }
}
