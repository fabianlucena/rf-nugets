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
        public async Task<IEnumerable<long>> GetAllPermissionsIdForRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null)
        {
            return await permissionXRoleRepository.GetAllPermissionsIdByRoleIdsAsync(roleIds, options);
        }

        public async Task<IEnumerable<string>> GetAllPermissionNamesForRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null)
        {
            var allPermissionsId = await GetAllPermissionsIdForRoleIdsAsync(roleIds, options);
            return await permissionService.GetListNameByIdAsync(allPermissionsId);
        }
    }
}
