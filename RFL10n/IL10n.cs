namespace RFL10n
{
    public interface IL10n
    {
        void AddTranslator(IL10nTranslator translator);

        void AddTranslation(string language, string context, string text, string translation);

        void AddTranslationsFromFile(string language, string context, string filename);

        Task<string> _(string text, params string[] args);

#pragma warning disable IDE1006 // Naming Styles
        Task<string> _c(string context, string text, params string[] args);
#pragma warning restore IDE1006 // Naming Styles
    }
}
