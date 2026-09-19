using Microsoft.Extensions.DependencyInjection;

namespace RFDapper
{
    public static class Setup
    {
        public static void CreateTable<TEntity>(IServiceProvider services)
            where TEntity : class
        {
            var dapperService = services.GetService<Dapper<TEntity>>() ??
                throw new Exception($"No service {typeof(TEntity).Name}");

            dapperService.CreateTable();
        }
    }
}
