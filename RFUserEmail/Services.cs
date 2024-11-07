using Microsoft.Extensions.DependencyInjection;
using RFService.IService;
using RFService.Services;
using RFUserEmail.IServices;
using RFUserEmail.Services;

namespace RFUserEmail
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFUserEmail(this IServiceCollection services)
        {
            services.AddScoped<IUserEmailService, UserEmailService>();

            services.AddSingleton<IPropertiesDecorators, PropertiesDecorators>();
        }
    }
}
