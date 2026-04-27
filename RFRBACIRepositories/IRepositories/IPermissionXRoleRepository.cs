using RFBaseIRepositories.IRepositories;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIRepositories.IRepositories
{
    public interface IPermissionXRoleRepository : ICommonJoinRepository<PermissionXRole>
    {
        Task<IEnumerable<long>> GetAllPermissionsIdByRolesIdAsync(IEnumerable<long> rolesId, PermissionXRoleQueryOptions? options = null);
    }
}