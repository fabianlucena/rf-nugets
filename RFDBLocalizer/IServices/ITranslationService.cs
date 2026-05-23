using RFDBLocalizer.Entities;
using RFIServices.IServices;

namespace RFDBLocalizer.IServices
{
    public interface ITranslationService : ICommonEntityService<Translation>
    {
        Task<string?> GetTranslationAsync(string language, string context, string source);
    }
}
