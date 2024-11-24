using RFRBAC.Entities;
using RFService.IServices;

namespace RFRBAC.IServices
{
    public interface IRoleService
        : IService<Role>,
            IServiceName<Role>,
            IServiceIdName<Role>,
            IServiceDecorated
    {
    }
}
