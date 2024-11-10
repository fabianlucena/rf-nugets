using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFRBAC.Entities;
using RFService.IRepo;

namespace RFRBACDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFRBACDapper(this IServiceCollection services)
        {
            services.AddScoped<Dapper<Role>, Dapper<Role>>();
            services.AddScoped<Dapper<Permission>, Dapper<Permission>>();
            services.AddScoped<Dapper<RolePermission>, Dapper<RolePermission>>();
            services.AddScoped<Dapper<UserRole>, Dapper<UserRole>>();
            services.AddScoped<Dapper<RoleParent>, Dapper<RoleParent>>();

            services.AddScoped<IRepo<Role>, Dapper<Role>>();
            services.AddScoped<IRepo<Permission>, Dapper<Permission>>();
            services.AddScoped<IRepo<RolePermission>, Dapper<RolePermission>>();
            services.AddScoped<IRepo<UserRole>, Dapper<UserRole>>();
            services.AddScoped<IRepo<RoleParent>, Dapper<RoleParent>>();
        }
    }
}
