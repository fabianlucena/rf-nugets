using RFLocalizer.Entities;
using RFService.IServices;

namespace RFLocalizer.IServices
{
    public interface ITranslationService
        : IService<Translation>,
            IServiceId<Translation>
    {
        Task<string> GetTranslationAsync(
            string language,
            string context,
            string source
        );
    }
}
