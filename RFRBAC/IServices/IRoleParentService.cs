using RFRBAC.Entities;
using RFService.IServices;
using RFService.Repo;

namespace RFRBAC.IServices
{
    public interface IRoleParentService
        : IServiceTimestamps<RoleParent>
    {
        Task<IEnumerable<Int64>> GetAllRolesIdForRolesIdAsync(IEnumerable<Int64> rolesId, GetOptions? options = null);
    }
}
