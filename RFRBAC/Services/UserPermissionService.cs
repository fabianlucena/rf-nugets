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
            GetOptions? options = null
        )
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUserIdAsync(userId);
            return await rolePermissionService.GetPermissionsForRoleIdListAsync(allRolesId);
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsForUsersIdAsync(
            IEnumerable<Int64> usersId,
            GetOptions? options = null
        )
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUsersIdAsync(usersId);
            return await rolePermissionService.GetPermissionsForRoleIdListAsync(allRolesId);
        }

        public async Task<bool> UserIdHasAnyPermissionAsync(Int64 userId, params string[] checkingPermissions)
        {
            var rolesName = (await userRoleService.GetAllRolesNameForUserIdAsync(userId)).ToList();
            return checkingPermissions.Any(i => rolesName.Contains(i));
        }
    }
}
