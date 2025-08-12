using RFRBAC.Entities;
using RFService.IServices;
using RFService.Repo;

namespace RFRBAC.IServices
{
    public interface IRoleParentService
        : IService<RoleParent>,
            IServiceTimestamps<RoleParent>
    {
        Task<IEnumerable<Int64>> GetAllRolesIdForRolesIdAsync(IEnumerable<Int64> rolesId, QueryOptions? options = null);
    }
}
