using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFDBLocalizer.Entities;
using RFLocalizer.Entities;

namespace RFDBLocalizerDapper
{
    public static class Setup
    {
        public static void ConfigureRFDBLocalizerDapper(IServiceProvider services)
        {
            CreateTable<Language>(services);
            CreateTable<Context>(services);
            CreateTable<Source>(services);
            CreateTable<Translation>(services);
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