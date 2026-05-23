using RFPermissions.IServices;
using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;
using RFServices.Services;

namespace RFRBAC.Services
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
