using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFRBAC.Entities;

namespace RFRBACDapper
{
    public static class Setup
    {
        public static void ConfigureRFRBACDapper(IServiceProvider services)
        {
            CreateTable<Role>(services);
            CreateTable<Permission>(services);
            CreateTable<RolePermission>(services);
            CreateTable<UserRole>(services);
            CreateTable<RoleParent>(services);
        }

        public static void CreateTable<Entity>(IServiceProvider services)
            where Entity : class
        {
            var dapperService = services.GetService<Dapper<Entity>>() ??
                throw new Exception($"No service {typeof(Entity).Name}");

            dapperService.CreateTable();
        }
    }
}