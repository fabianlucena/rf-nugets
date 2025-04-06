using System.Globalization;

namespace RFL10n
{
    public class L10n(IServiceProvider provider, string acceptLanguage)
        : IL10n
    {
        static private readonly List<IL10nTranslator> Translators = [];

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

        public async Task<string> _(string text, params string[] args)
        {
            var translation = (await GetTranslation(text)) ?? text;
            return string.Format(translation, args);
        }

        public async Task<string?> GetTranslation(string text, string context = "")
        {
            foreach (var language in Languages)
            {
                foreach (var translator in Translators)
                {
                    var result = await translator.GetTranslationAsync(provider, text, language, context);
                    if (result != null)
                        return result;
                }
            }

            return text;
        }
    }
}
