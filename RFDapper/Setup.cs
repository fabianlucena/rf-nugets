using Microsoft.Extensions.DependencyInjection;

namespace RFDapper
{
    public static class Setup
    {
        public static void CreateTable<Entity>(IServiceProvider services)
            where Entity : class
        {
            var dapperService = services.GetService<Dapper<Entity>>() ??
                throw new Exception($"No service {typeof(Entity).Name}");

            dapperService.CreateTable();
        }
    }
}
