using RFRBAC.Entities;
using RFService.IServices;
using RFService.Repo;

namespace RFRBAC.IServices
{
    public interface IRoleService
        : IService<Role>, IServiceName<Role>
    {
    }
}
