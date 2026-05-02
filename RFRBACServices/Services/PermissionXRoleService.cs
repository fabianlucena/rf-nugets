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
        public async Task<IEnumerable<long>> GetPermissionIdsByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null)
            => await permissionXRoleRepository.GetPermissionIdsByRoleIdsAsync(roleIds, options);

        public async Task<IEnumerable<string>> GetPermissionNamesByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null)
        {
            var allPermissionIds = await GetPermissionIdsByRoleIdsAsync(roleIds, options);
            return await permissionService.GetNamesByIdsAsync(allPermissionIds);
        }
    }
}
