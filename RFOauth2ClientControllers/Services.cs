using Microsoft.Extensions.DependencyInjection;
using RFOauth2Client.IServices;
using RFOauth2Client.Service;

namespace RFOauth2Client
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFOauth2Client(this IServiceCollection services)
        {
            services.AddScoped<IProviderService, ProviderService>();

            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        }
    }
}