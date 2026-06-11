using System.Globalization;

namespace RFL10n;

public class L10n(IServiceProvider provider, string acceptLanguage)
    : IL10n
{
    static private readonly List<IL10nTranslator> Translators = [];
    static private readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> Cache = [];
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

    static public void AddTranslator(IL10nTranslator translator)
        => Translators.Add(translator);

    public static void AddToCache(string language, string context, string text, string translation)
    {
        if (!Cache.TryGetValue(language, out var tables))
        {
            tables = [];
            Cache[language] = tables;
        }

        if (!tables.TryGetValue(context, out var table))
        {
            table = [];
            tables[context] = table;
        }

        table[text] = translation;
    }

    public static void AddTranslation(string language, string context, string text, string translation)
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

    public static void AddTranslationsFromFile(string language, string context, string filename)
    {
        try
        {
            string translations = File.ReadAllText(filename).Trim();
            if (string.IsNullOrEmpty(translations))
                return;

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

            string[] lines = translations.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i += 2)
            {
                string clave = lines[i].Trim();
                string valor = lines[i + 1].Trim();
                table[clave] = valor;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static void AddTranslationsFromPath(string path, bool absolutePath = false, bool throwIfNoFiles = false)
    {
        var indent = "      ";
        if (!absolutePath)
            path = Path.Combine(AppContext.BaseDirectory, path);

        if (!Directory.Exists(path))
        {
            if (throwIfNoFiles)
                throw new DirectoryNotFoundException($"The directory {path} does not exist.");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("warn: ");
            Console.ResetColor();
            Console.WriteLine("No translations files in path:");
            Console.Write(indent);
            Console.WriteLine(path);
            return;
        }

        var files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
        if (files.Length == 0)
            return;

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("info: ");
        Console.ResetColor();
        Console.WriteLine("Loading translations from files:");
        foreach (var file in files)
        {
            Console.Write(indent);
            Console.WriteLine(file);

            var filename = Path.GetFileNameWithoutExtension(file);
            var parts = filename.Split('_');
            if (parts.Length == 2)
            {
                var language = parts[0];
                var context = parts[1];
                AddTranslationsFromFile(language, context, file);
            }
            else if (parts.Length == 1)
            {
                var language = parts[0];
                AddTranslationsFromFile(language, "", file);
            }
            else
                throw new DirectoryNotFoundException($"The filaname {file} is not normalized.");
        }
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
            if (Cache.TryGetValue(language, out var tables)
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
                    AddToCache(language, context, text, result);
                    return result;
                }
            }

            if (Translations.TryGetValue(language, out tables)
                && tables.TryGetValue(context, out table)
                && table.TryGetValue(text, out translation))
            {
                return translation;
            }
        }

        return text;
    }
}
