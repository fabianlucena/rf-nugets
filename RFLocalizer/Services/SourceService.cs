using RFLocalizer.Entities;
using RFLocalizer.IServices;
using RFService.IRepo;
using RFService.Services;

namespace RFLocalizer.Services
{
    public class SourceService(
        IRepo<Source> repo
    )
        : ServiceIdUuidName<IRepo<Source>, Source>(repo),
            ISourceService
    {
    }
}
