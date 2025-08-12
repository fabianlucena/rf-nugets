using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.Repo;

namespace RFRBAC.Services
{
    public class UserPermissionService(
        IUserRoleService userRoleService,
        IRolePermissionService rolePermissionService
    ) : IUserPermissionService
    {
        public async Task<IEnumerable<Permission>> GetAllPermissionsForUserIdAsync(
            Int64 userId,
            QueryOptions? options = null
        )
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUserIdAsync(userId);
            return await rolePermissionService.GetPermissionsForRoleIdListAsync(allRolesId);
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsForUsersIdAsync(
            IEnumerable<Int64> usersId,
            QueryOptions? options = null
        )
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUsersIdAsync(usersId);
            return await rolePermissionService.GetPermissionsForRoleIdListAsync(allRolesId);
        }

        public async Task<IEnumerable<string>> GetAllPermissionsNameForUserIdAsync(
            Int64 usersId,
            QueryOptions? options = null
        )
        {
            var allPermissions = await GetAllPermissionsForUserIdAsync(usersId, options);
            return allPermissions.Select(i => i.Name);
        }

        public async Task<bool> UserIdHasAnyPermissionAsync(Int64 userId, params string[] checkingPermissions)
        {
            var permissionsName = (await GetAllPermissionsNameForUserIdAsync(userId)).ToList();
            return checkingPermissions.Any(i => permissionsName.Contains(i));
        }
    }
}
