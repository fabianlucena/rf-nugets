using RFDBLocalizer.Entities;
using RFService.IServices;

namespace RFDBLocalizer.IServices
{
    public interface IContextService
        : IService<Context>,
            IServiceName<Context>,
            IServiceIdUuidName<Context>
    {
    }
}
