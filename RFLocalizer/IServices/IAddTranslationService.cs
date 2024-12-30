namespace RFLocalizer.IServices
{
    public interface IAddTranslationService
    {
        Task<bool> AddAsync(
            string language,
            string context,
            string source,
            string translation
        );
    }
}
