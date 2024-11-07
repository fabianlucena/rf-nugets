using Microsoft.Extensions.DependencyInjection;
using RFAuth.Entities;
using RFDapper;

namespace RFAuthDapper
{
    public static class Setup
    {
        public static void ConfigureRFAuthDapper(IServiceProvider services)
        {
            CreateTable<Device>(services);
            CreateTable<UserType>(services);
            CreateTable<User>(services);
            CreateTable<Password>(services);
            CreateTable<Session>(services);
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
