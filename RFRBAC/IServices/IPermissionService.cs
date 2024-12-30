using RFRBAC.Entities;
using RFService.IServices;

namespace RFRBAC.IServices
{
    public interface IPermissionService
        : IService<Permission>,
            IServiceId<Permission>,
            IServiceName<Permission>,
            IServiceIdUuidName<Permission>
    {
    }
}
