using Microsoft.Extensions.Primitives;
using RFLocalizer.IServices;

namespace RFLocalizer.Services
{
    public class LocalizerFactoryService(
        IServiceProvider provider
    )
        : ILocalizerFactoryService
    {
        private static readonly Dictionary<string, Dictionary<string, LocalizerService>> LocalizerByLanguages = [];

        public ILocalizerService GetLocalizer(string language, string context)
        {
            if (LocalizerByLanguages.TryGetValue(language, out var contexts))
            {
                if (contexts.TryGetValue(context, out var value))
                    return value;
            }
            else
            {
                contexts = [];
                LocalizerByLanguages[language] = contexts;
            }

            var localizer = new LocalizerService(provider, language, context);
            contexts[context] = localizer;

            return localizer;
        }

        public ILocalizerService GetLocalizerForAcceptLanguage(StringValues acceptLanguage, string context)
        {
            var acceptLanguages = acceptLanguage.ToString()
               .Split(',')
               .Select(lang =>
               {
                   var parts = lang.Split(';');
                   var language = parts[0].Trim();
                   if (parts.Length < 2
                       || !parts[1].Trim().StartsWith("q=")
                       || !double.TryParse(parts[1].Trim().AsSpan(2), out var qValue)
                   )
                       qValue = 1.0;

                   return new { Language = language, Quality = qValue };
               })
               .OrderByDescending(lp => lp.Quality)
               .ToList();

            return GetLocalizer(acceptLanguages[0].Language, context);
        }
    }
}
