using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFHttpAction.Entities;

namespace RFHttpActionDapper
{
    public static class Setup
    {
        public static void ConfigureRFHttpActionDapper(IServiceProvider services)
        {
            CreateTable<HttpActionType>(services);
            CreateTable<HttpAction>(services);
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
