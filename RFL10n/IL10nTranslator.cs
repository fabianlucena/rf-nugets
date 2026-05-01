namespace RFL10n
{
    public interface IL10nTranslator
    {
        Task<string?> GetTranslationAsync(IServiceProvider provider, string text, string language, string context = "");
    }
}
