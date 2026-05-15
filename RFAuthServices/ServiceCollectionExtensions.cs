using Microsoft.Extensions.DependencyInjection;
using RFAuthIServices.IServices;
using RFAuthServices.Services;

namespace RFAuthServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFAuthServices(this IServiceCollection services)
        {
            services.AddScoped<IUserPasswordService, UserPasswordService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ILoginService, LoginService>();

            return services;
        }

    }
}
