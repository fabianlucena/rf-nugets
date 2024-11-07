using Microsoft.Extensions.DependencyInjection;
using RFAuth.IServices;
using RFAuth.Services;

namespace RFAuth
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFAuth(this IServiceCollection services)
        {
            services.AddScoped<IUserTypeService, UserTypeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ILoginService, LoginService>();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
    }
}