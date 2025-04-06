using Microsoft.Extensions.DependencyInjection;
using RFDBLocalizer.IServices;
using RFDBLocalizer.Services;

namespace RFDBLocalizer
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFDBLocalizer(this IServiceCollection services)
        {
            services.AddScoped<IDBTranslator, DBTranslator>();
            services.AddScoped<ITranslationService, TranslationService>();
        }
    }
}