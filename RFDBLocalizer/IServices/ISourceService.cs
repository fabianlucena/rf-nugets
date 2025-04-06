using RFDBLocalizer.Entities;
using RFService.IServices;

namespace RFDBLocalizer.IServices
{
    public interface ISourceService
        : IService<Source>,
            IServiceName<Source>,
            IServiceIdUuidName<Source>
    {
    }
}
