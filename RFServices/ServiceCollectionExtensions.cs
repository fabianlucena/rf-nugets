using Microsoft.Extensions.DependencyInjection;
using RFIServices.IServices;
using RFServices.Services;

namespace RFServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFBaseServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
