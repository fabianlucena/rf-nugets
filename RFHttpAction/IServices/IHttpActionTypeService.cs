using RFHttpAction.Entities;
using RFService.IService;

namespace RFHttpAction.IServices
{
    public interface IHttpActionTypeService
        : IServiceId<HttpActionType>,
            IServiceName<HttpActionType>,
            IServiceIdName<HttpActionType>
    {
    }
}
