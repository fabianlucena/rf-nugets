using Microsoft.Extensions.DependencyInjection;
using RFDBLocalizer.IServices;
using RFDBLocalizer.Services;

namespace RFDBLocalizer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFDBLocalizerServices(this IServiceCollection services)
        {
            services.AddScoped<IDBTranslator, DBTranslator>();
            services.AddScoped<ITranslationService, TranslationService>();

            return services;
        }
    }
}