using Microsoft.Extensions.DependencyInjection;
using RFOauth2Client.IServices;
using RFOauth2Client.Service;

namespace RFOauth2Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFOauth2ClientServices(this IServiceCollection services)
        {
            services.AddScoped<IProviderService, ProviderService>();

            return services;
        }
    }
}