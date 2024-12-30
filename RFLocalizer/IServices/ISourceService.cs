using RFLocalizer.Entities;
using RFService.IServices;

namespace RFLocalizer.IServices
{
    public interface ISourceService
        : IService<Source>,
            IServiceName<Source>,
            IServiceIdUuidName<Source>
    {
    }
}
