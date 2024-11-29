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

        public static void CreateTable<TEntity>(IServiceProvider services)
            where TEntity : class
        {
            var dapperService = services.GetService<Dapper<TEntity>>() ??
                throw new Exception($"No service {typeof(TEntity).Name}");

            dapperService.CreateTable();
        }
    }
}