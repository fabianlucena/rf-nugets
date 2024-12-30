using RFRBAC.Entities;
using RFService.IServices;

namespace RFRBAC.IServices
{
    public interface IRoleService
        : IService<Role>,
            IServiceName<Role>,
            IServiceIdUuidName<Role>,
            IServiceDecorated
    {
    }
}
