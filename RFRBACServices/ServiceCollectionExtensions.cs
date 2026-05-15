using Microsoft.Extensions.DependencyInjection;
using RFRBACIServices.IServices;
using RFRBACServices.Services;

namespace RFRBACServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFRBACServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleIncludeService, RoleIncludeService>();
            services.AddScoped<IRoleXUserService, RoleXUserService>();
            services.AddScoped<IPermissionXRoleService, PermissionXRoleService>();

            return services;
        }

    }
}
