using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFLocalizer.Entities;

namespace RFLocalizerDapper
{
    public static class Setup
    {
        public static void ConfigureRFLocalizerDapper(IServiceProvider services)
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