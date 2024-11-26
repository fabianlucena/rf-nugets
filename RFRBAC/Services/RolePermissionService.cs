using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace RFRBAC.Services
{
    public class RolePermissionService(
        IRepo<RolePermission> repo,
        IPermissionService permissionService,
        IUserRoleService userRoleService
    ) : ServiceTimestamps<IRepo<RolePermission>, RolePermission>(repo),
            IRolePermissionService
    {
        public async Task<IEnumerable<Int64>> GetPermissionsIdForRolesIdAsync(
            IEnumerable<Int64> rolesId,
            GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["RoleId"] = rolesId;
            var rolesPermissions = await GetListAsync(options);
            return rolesPermissions.Select(i => i.PermissionId);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsForRolesIdAsync(
            IEnumerable<Int64> rolesId,
            GetOptions? options = null)
        {
            var permissionsId = await GetPermissionsIdForRolesIdAsync(rolesId, options);
            return await permissionService.GetListForIdsAsync(permissionsId);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsForRoleIdListAsync(
            IEnumerable<Int64> rolesId,
            GetOptions? options = null
        )
        {
            var permissionsId = await GetPermissionsIdForRolesIdAsync(rolesId);

            options ??= new GetOptions();
            options.Filters["Id"] = permissionsId;

            return await permissionService.GetListAsync(options);
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsForUserIdAsync(
            Int64 userId,
            GetOptions? options = null
        )
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUserIdAsync(userId);
            return await GetPermissionsForRoleIdListAsync(allRolesId);
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsForUsersIdAsync(
            IEnumerable<Int64> usersId,
            GetOptions? options = null
        )
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUsersIdAsync(usersId);
            return await GetPermissionsForRoleIdListAsync(allRolesId);
        }
    }
}
