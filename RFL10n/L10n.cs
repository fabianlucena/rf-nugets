using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace RFL10n
{
    public class L10n(IServiceProvider provider, string acceptLanguage)
        : IL10n
    {
        static private readonly List<IL10nTranslator> Translators = [];
        static private readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> Translations = [];

        private readonly string[] Languages = [
            ..acceptLanguage.Split(',')
                .Select(language =>
                {
                    var keyValue = language.Split(';');
                    var key = keyValue[0].Trim();
                    float value;
                    if (keyValue.Length > 1)
                    {
                        if (keyValue[1].Trim().StartsWith("q="))
                            value = float.Parse(keyValue[1].Trim().AsSpan(2), CultureInfo.InvariantCulture);
                        else
                            value = 1;

                        return new KeyValuePair<string, float>(key, value);
                    }
                    else
                        value = 1;

                    return new KeyValuePair<string, float>(key, value);
                })
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
            ];

        public void AddTranslator(IL10nTranslator translator)
            => Translators.Add(translator);

        public void AddTranslation(string language, string context, string text, string translation)
        {
            if (!Translations.TryGetValue(language, out var tables))
            {
                tables = [];
                Translations[language] = tables;
            }

            if (!tables.TryGetValue(context, out var table))
            {
                table = [];
                tables[context] = table;
            }

            table[text] = translation;
        }

        public async Task<string> _(string text, params string[] args)
        {
            var translation = (await GetTranslation(text)) ?? text;
            return string.Format(translation, args);
        }

        public async Task<string> _c(string context, string text, params string[] args)
        {
            var translation = (await GetTranslation(text, context)) ?? text;
            return string.Format(translation, args);

        }

        public async Task<string?> GetTranslation(string text, string context = "")
        {
            foreach (var language in Languages)
            {
                if (Translations.TryGetValue(language, out var tables)
                    && tables.TryGetValue(context, out var table)
                    && table.TryGetValue(text, out var translation))
                {
                    return translation;
                }

                foreach (var translator in Translators)
                {
                    var result = await translator.GetTranslationAsync(provider, text, language, context);
                    if (result != null)
                    {
                        AddTranslation(language, context, text, result);
                        return result;
                    }
                }
            }

            return text;
        }
    }
}
