using Microsoft.Extensions.DependencyInjection;
using RFBaseIServices.IServices;
using RFBaseServices.Services;

namespace RFBaseServices
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
