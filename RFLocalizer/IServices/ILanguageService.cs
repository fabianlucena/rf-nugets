using RFLocalizer.Entities;
using RFService.IServices;

namespace RFLocalizer.IServices
{
    public interface ILanguageService
        : IService<Language>,
            IServiceName<Language>,
            IServiceIdUuidName<Language>
    {
    }
}
