using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RFRegisterService.Attributes;
using System.Globalization;

namespace RFL10n;

[RegisterService]
public class L10n(IServiceProvider provider)
    : IL10n
{
    static private readonly List<IL10nTranslator> Translators = [];
    static private readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> Cache = [];
    static private readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> Translations = [];

    private string? _acceptLanguage;
    private string[]? _languages;

    public string AcceptLanguage
    {
        get
        {
            if (_acceptLanguage == null)
            {
                var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                var httpContext = httpContextAccessor?.HttpContext;
                if (httpContext != null)
                    _acceptLanguage = httpContext.Request.Headers["Accept-Language"].ToString();
            }

            return _acceptLanguage ?? "";
        }
    }

    public string[] Languages
    {
        get
        {
            _languages ??= [
                ..AcceptLanguage.Split(',')
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

            return _languages;
        }
    }

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

    public static void AddTranslationsFromPath(string path = "locale", bool absolutePath = false, bool throwIfNoFiles = false)
    {
        var basePath = Path.Combine(absolutePath ? "" : AppContext.BaseDirectory, path);
        var indent = "      ";
        var showPath = absolutePath ? "" : ".";

        if (!Directory.Exists(basePath))
        {
            if (throwIfNoFiles)
                throw new DirectoryNotFoundException($"The directory {path} does not exist.");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("warn: ");
            Console.ResetColor();
            Console.WriteLine($"No translations files in path: {showPath + Path.Combine(path, "*.txt")}");
            return;
        }

        var files = Directory.GetFiles(basePath, "*.txt", SearchOption.AllDirectories);
        if (files.Length == 0)
            return;

        var pathFrom = basePath.Length;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("info: ");
        Console.ResetColor();
        Console.WriteLine($"Loading translations from path: {showPath + Path.Combine(path, "*.txt")}");
        foreach (var file in files)
        {
            var localFileName = file[pathFrom..];

            Console.Write(indent);
            Console.WriteLine(string.Concat(showPath, localFileName));

            var localPath = Path.GetDirectoryName(localFileName) ?? "";
            while (localPath.StartsWith(Path.DirectorySeparatorChar))
                localPath = localPath[1..];

            var parts = localPath.Split(Path.DirectorySeparatorChar);
            var language = parts[0];
            var context = parts.Length > 1 ? parts[1] : Path.GetFileNameWithoutExtension(localFileName);
            AddTranslationsFromFile(language, context, file);
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

            if (Translations.TryGetValue(language, out tables)
                && tables.TryGetValue("", out table)
                && table.TryGetValue(text, out translation))
            {
                return translation;
            }
        }

        return text;
    }
}
