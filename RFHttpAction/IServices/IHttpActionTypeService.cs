using RFHttpAction.Entities;
using RFService.IServices;

namespace RFHttpAction.IServices
{
    public interface IHttpActionTypeService
        : IServiceId<HttpActionType>,
            IServiceName<HttpActionType>,
            IServiceIdName<HttpActionType>
    {
    }
}
