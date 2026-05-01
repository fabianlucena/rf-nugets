using RFBaseIRepositories.IRepositories;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIRepositories.IRepositories
{
    public interface IPermissionXRoleRepository : ICommonJoinRepository<PermissionXRole>
    {
        Task<IEnumerable<long>> GetAllPermissionsIdByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null);
    }
}