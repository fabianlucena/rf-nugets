using RFLocalizer.Entities;
using RFService.IServices;

namespace RFDBLocalizer.IServices
{
    public interface ILanguageService
        : IService<Language>,
            IServiceName<Language>,
            IServiceIdUuidName<Language>
    {
    }
}
