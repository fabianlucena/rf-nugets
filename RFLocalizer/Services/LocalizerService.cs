using Microsoft.Extensions.DependencyInjection;
using RFLocalizer.IServices;
using RFService.IRepo;
using RFService.Services;
using System.Data;

namespace RFLocalizer.Services
{
    public class LocalizerService(
        IServiceProvider provider,
        string language,
    string context
    )
        : ILocalizerService
    {
        private readonly Dictionary<string, string> Cache = [];

        private readonly ITranslationService translationService = provider.GetRequiredService<ITranslationService>();

        public string this[string text]
            => GetTranslationAsync(text)
                .GetAwaiter()
                .GetResult();

        public async Task<string> GetTranslationAsync(string source) {
            if (Cache.TryGetValue(source, out var translation))
                return translation;

            translation = await translationService.GetTranslationAsync(language, context, source);
            Cache[source] = translation;

            return translation;
        }
    }
}
