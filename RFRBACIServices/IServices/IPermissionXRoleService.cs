using RFBaseIServices.IServices;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIServices.IServices
{
    public interface IPermissionXRoleService : ICommonJoinService<PermissionXRole>
    {
        Task<IEnumerable<long>> GetAllPermissionsIdForRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null);
        Task<IEnumerable<string>> GetAllPermissionNamesForRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null);
    }
}
