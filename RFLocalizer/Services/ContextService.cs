using RFLocalizer.Entities;
using RFLocalizer.IServices;
using RFService.IRepo;
using RFService.Services;

namespace RFLocalizer.Services
{
    public class ContextService(
        IRepo<Context> repo
    )
        : ServiceIdUuidName<IRepo<Context>, Context>(repo),
            IContextService
    {
    }
}
