using RFDBLocalizer.IServices;
using RFLocalizer.Entities;
using RFService.IRepo;
using RFService.Services;

namespace RFDBLocalizer.Services
{
    public class LanguageService(
        IRepo<Language> repo
    )
        : ServiceIdUuidName<IRepo<Language>, Language>(repo),
            ILanguageService
    {
    }
}
