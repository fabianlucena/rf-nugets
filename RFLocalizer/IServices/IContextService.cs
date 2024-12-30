using RFLocalizer.Entities;
using RFService.IServices;

namespace RFLocalizer.IServices
{
    public interface IContextService
        : IService<Context>,
            IServiceName<Context>,
            IServiceIdUuidName<Context>
    {
    }
}
