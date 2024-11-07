using RFService.Services;
using RFService.IRepo;
using RFHttpAction.Entities;
using RFHttpAction.IServices;

namespace RFHttpAction.Services
{
    public class HttpActionTypeService(IRepo<HttpActionType> repo)
        : ServiceTimestampsIdUuidEnabledNameTitleTranslatable<IRepo<HttpActionType>, HttpActionType>(repo),
            IHttpActionTypeService
    {
    }
}
