using Microsoft.Extensions.DependencyInjection;
using RFLocalizer.IServices;
using RFLocalizer.Services;

namespace RFLocalizer
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFLocalizer(this IServiceCollection services)
        {
            services.AddScoped<ILocalizerFactoryService, LocalizerFactoryService>();

            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IContextService, ContextService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<ITranslationService, TranslationService>();
            services.AddScoped<IAddTranslationService, AddTranslationService>();
        }
    }
}