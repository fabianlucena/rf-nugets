using Microsoft.Extensions.DependencyInjection;
using RFAuthEF.Repositories;
using RFAuth.IRepositories;

namespace RFAuthEF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFAuthEF(this IServiceCollection services)
        {
            services.AddScoped<IUserPasswordRepository, UserPasswordRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();

            return services;
        }

    }
}
