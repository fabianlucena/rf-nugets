using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFRBAC.DTO;
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

        public static void CreateTable<TEntity>(IServiceProvider services)
            where TEntity : class
        {
            var dapperService = services.GetService<Dapper<TEntity>>() ??
                throw new Exception($"No service {typeof(TEntity).Name}");

            dapperService.CreateTable();
        }
    }
}