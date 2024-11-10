using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFRGBAC.Entities;

namespace RFRGBACDapper
{
    public static class Setup
    {
        public static void ConfigureRFRGBACDapper(IServiceProvider services)
        {
            CreateTable<UserGroup>(services);
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