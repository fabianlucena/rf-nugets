using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace RFRBAC.Services
{
    public class UserRoleService(
        IRepo<UserRole> repo,
        IRoleParentService roleParentService
    ) : ServiceTimestamps<IRepo<UserRole>, UserRole>(repo),
            IUserRoleService
    {
        public async Task<IEnumerable<Int64>> GetRolesIdAsync(GetOptions options)
        {
            var roles = await GetListAsync(options);
            return roles.Select(i => i.RoleId);
        }

        public async Task<IEnumerable<Int64>> GetRolesIdForUserIdAsync(Int64 userId, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["UserId"] = userId;

            return await GetRolesIdAsync(options);
        }

        public async Task<IEnumerable<Int64>> GetRolesIdForUsersIdAsync(IEnumerable<Int64> usersId, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["UserId"] = usersId;

            return await GetRolesIdAsync(options);
        }

        public async Task<IEnumerable<Int64>> GetAllRolesIdForUserIdAsync(Int64 userId, GetOptions? options = null)
        {
            var rolesId = await GetRolesIdForUserIdAsync(userId, options);
            var allRolesId = await roleParentService.GetAllRolesIdForRolesIdAsync(rolesId);

            return allRolesId;
        }

        public async Task<IEnumerable<Int64>> GetAllRolesIdForUsersIdAsync(IEnumerable<Int64> usersId, GetOptions? options = null)
        {
            var rolesId = await GetRolesIdForUsersIdAsync(usersId, options);
            var allRolesId = await roleParentService.GetAllRolesIdForRolesIdAsync(rolesId);

            return allRolesId;
        }
    }
}
