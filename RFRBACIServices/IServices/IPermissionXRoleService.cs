using RFBaseIServices.IServices;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIServices.IServices
{
    public interface IPermissionXRoleService : ICommonJoinService<PermissionXRole>
    {
        Task<IEnumerable<long>> GetAllPermissionsIdForRolesIdAsync(IEnumerable<long> rolesId, PermissionXRoleQueryOptions? options = null);
        Task<IEnumerable<string>> GetAllPermissionsNamesForRolesIdAsync(IEnumerable<long> rolesId, PermissionXRoleQueryOptions? options = null);
    }
}
