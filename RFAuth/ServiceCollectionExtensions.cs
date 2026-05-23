using Microsoft.Extensions.DependencyInjection;
using RFAuth.IServices;
using RFAuth.Services;

namespace RFAuth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFAuth(this IServiceCollection services)
        {
            services.AddScoped<IUserPasswordService, UserPasswordService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ILoginService, LoginService>();

            return services;
        }

    }
}
