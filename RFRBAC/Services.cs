using Microsoft.Extensions.DependencyInjection;
using RFAuth.IServices;
using RFRBAC.IServices;
using RFRBAC.Services;

namespace RFRBAC
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFRBAC(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleParentService, RoleParentService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IAddRolePermissionService, RolePermissionService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserPermissionService, UserPermissionService>();
            services.AddScoped<IPrivilegeService, PrivilegeService>();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
    }
}