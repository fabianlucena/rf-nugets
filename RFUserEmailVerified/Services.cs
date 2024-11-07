using Microsoft.Extensions.DependencyInjection;
using RFService.IService;
using RFService.Services;
using RFUserEmailVerified.IServices;
using RFUserEmailVerified.Services;

namespace RFUserEmailVerified
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFUserEmailVerified(this IServiceCollection services)
        {
            services.AddScoped<IUserEmailVerifiedService, UserEmailVerifiedService>();

            services.AddSingleton<IPropertiesDecorators, PropertiesDecorators>();
        }
    }
}
