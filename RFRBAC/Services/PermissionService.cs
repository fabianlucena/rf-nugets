using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace RFRBAC.Services
{
    public class PermissionService(
        IRepo<Permission> repo,
        IUserRoleService userRoleService,
        IRolePermissionService rolePermissionService
    ) : ServiceSoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable<IRepo<Permission>, Permission>(repo),
        IPermissionService
    {
        public async Task<IEnumerable<Permission>> GetListForRoleIdListAsync(
            IEnumerable<Int64> rolesId,
            GetOptions? options = null
        )
        {
            var permissionsId = await rolePermissionService.GetPermissionsIdForRolesIdAsync(rolesId);

            options ??= new GetOptions();
            options.Filters["Id"] = permissionsId;

            return await GetListAsync(options);
        }

        public async Task<IEnumerable<Permission>> GetAllForUserIdAsync(
            Int64 userId,
            GetOptions? options = null
        )
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUserIdAsync(userId);
            var permissions = await GetListForRoleIdListAsync(allRolesId);

            return permissions;
        }

        public async Task<IEnumerable<Permission>> GetAllForUsersIdAsync(
            IEnumerable<Int64> usersId,
            GetOptions? options = null
        )
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUsersIdAsync(usersId);
            var permissions = await GetListForRoleIdListAsync(allRolesId);

            return permissions;
        }
    }
}
