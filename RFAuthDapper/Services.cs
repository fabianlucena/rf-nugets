using Microsoft.Extensions.DependencyInjection;
using RFAuth.Entities;
using RFDapper;
using RFService.IRepo;

namespace RFAuthDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFAuthDapper(this IServiceCollection services)
        {
            services.AddScoped<Dapper<UserType>, Dapper<UserType>>();
            services.AddScoped<Dapper<User>, Dapper<User>>();
            services.AddScoped<Dapper<Password>, Dapper<Password>>();
            services.AddScoped<Dapper<Device>, Dapper<Device>>();
            services.AddScoped<Dapper<Session>, Dapper<Session>>();

            services.AddScoped<IRepo<UserType>, Dapper<UserType>>();
            services.AddScoped<IRepo<User>, Dapper<User>>();
            services.AddScoped<IRepo<Password>, Dapper<Password>>();
            services.AddScoped<IRepo<Device>, Dapper<Device>>();
            services.AddScoped<IRepo<Session>, Dapper<Session>>();
        }
    }
}
