using Microsoft.Extensions.DependencyInjection;
using RFRBAC.IServices;
using RFRBAC.Services;

namespace RFRBAC
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFRBAC(this IServiceCollection services)
        {
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IRoleParentService, RoleParentService>();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
    }
}