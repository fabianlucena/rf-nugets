using Microsoft.Extensions.DependencyInjection;
using RFLocalizer.IServices;
using RFLocalizer.Services;

namespace RFLocalizer
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFLocalizer(this IServiceCollection services)
        {
            services.AddScoped<ILocalizerContextService, LocalizerContextService>();
        }
    }
}