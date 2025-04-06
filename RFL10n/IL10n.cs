namespace RFL10n
{
    public interface IL10n
    {
        void AddTranslator(IL10nTranslator translator);

        Task<string> _(string key, params string[] args);
    }
}
