using RFLocalizer.Entities;
using RFLocalizer.IServices;
using RFService.IRepo;
using RFService.IServices;
using RFService.Services;

namespace RFLocalizer.Services
{
    public class LanguageService(
        IRepo<Language> repo
    )
        : ServiceIdUuidName<IRepo<Language>, Language>(repo),
            ILanguageService
    {
    }
}
