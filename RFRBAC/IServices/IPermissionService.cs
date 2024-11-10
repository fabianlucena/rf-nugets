using RFRBAC.Entities;
using RFService.IServices;
using RFService.Repo;

namespace RFRBAC.IServices
{
    public interface IPermissionService
        : IService<Permission>, IServiceName<Permission>
    {
        Task<IEnumerable<Permission>> GetListForRoleIdListAsync(IEnumerable<Int64> rolesId, GetOptions? options = null);

        Task<IEnumerable<Permission>> GetAllForUserIdAsync(Int64 userId, GetOptions? options = null);
    }
}
