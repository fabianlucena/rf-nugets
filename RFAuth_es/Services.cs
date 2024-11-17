using Microsoft.Extensions.DependencyInjection;
using RFAuth.IServices;

namespace RFAuth_es
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFAuth_es(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddTransient<ILocalizerService, LocalizerService>();
        }
    }
}