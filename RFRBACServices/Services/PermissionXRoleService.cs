using RFBaseServices.Services;
using RFPermissionsIServices.IServices;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;
using RFRBACIRepositories.IRepositories;
using RFRBACIServices.IServices;

namespace RFRBACServices.Services
{
    public class PermissionXRoleService(
        IPermissionXRoleRepository permissionXRoleRepository,
        IPermissionService permissionService
    )
        : CommonJoinService<PermissionXRole>(permissionXRoleRepository),
        IPermissionXRoleService
    {
        public async Task<IEnumerable<long>> GetAllPermissionsIdForRolesIdAsync(IEnumerable<long> rolesId, PermissionXRoleQueryOptions? options = null)
        {
            return await permissionXRoleRepository.GetAllPermissionsIdByRolesIdAsync(rolesId, options);
        }

        public async Task<IEnumerable<string>> GetAllPermissionsNamesForRolesIdAsync(IEnumerable<long> rolesId, PermissionXRoleQueryOptions? options = null)
        {
            var allPermissionsId = await GetAllPermissionsIdForRolesIdAsync(rolesId, options);
            return await permissionService.GetListNameByIdAsync(allPermissionsId);
        }
    }
}
