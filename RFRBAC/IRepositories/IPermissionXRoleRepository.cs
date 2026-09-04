using RFIRepositories.IRepositories;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IRepositories
{
    public interface IPermissionXRoleRepository : ICommonJoinRepository<PermissionXRole>
    {
        Task<IEnumerable<long>> GetPermissionsIdByRolesIdAsync(IEnumerable<long> rolesId, PermissionXRoleQueryOptions? options = null);
    }
}