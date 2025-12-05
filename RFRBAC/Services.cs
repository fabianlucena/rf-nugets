using Microsoft.Extensions.DependencyInjection;
using RFAuth.IServices;
using RFAuth.Services;
using RFRBAC.IServices;
using RFRBAC.Services;

namespace RFRBAC
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFRBAC(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IRFRBACLoggerService, RFRBACLoggerService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleParentService, RoleParentService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IAddRolePermissionService, RolePermissionService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserPermissionService, UserPermissionService>();
            services.AddScoped<IPrivilegeService, PrivilegeService>();

            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        }
    }
}