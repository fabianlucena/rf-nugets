using RFDBLocalizer.Entities;
using RFService.IServices;

namespace RFDBLocalizer.IServices
{
    public interface ITranslationService
        : IService<Translation>,
            IServiceId<Translation>
    {
        Task<string?> GetTranslationAsync(
            string language,
            string context,
            string source
        );
    }
}
