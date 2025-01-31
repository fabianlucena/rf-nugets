using RFAuth.Entities;
using RFAuth.Services;
using RFRBAC.Entities;
using RFRBAC.Services;
using RFService.IServices;
using RFService.Repo;

namespace RFRBAC.IServices
{
    public interface IUserRoleService
        : IService<UserRole>,
            IServiceTimestamps<UserRole>
    {
        Task<IEnumerable<Int64>> GetRolesIdAsync(GetOptions options);

        Task<IEnumerable<Int64>> GetRolesIdForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<Int64>> GetRolesIdForUsersIdAsync(IEnumerable<Int64> usersId, GetOptions? options = null);

        Task<IEnumerable<Int64>> GetAllRolesIdForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<Int64>> GetAllRolesIdForUsersIdAsync(IEnumerable<Int64> usersId, GetOptions? options = null);

        Task<IEnumerable<Role>> GetRolesForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<Role>> GetAllRolesForUserIdAsync(Int64 userId, GetOptions? options = null);

        async Task<IEnumerable<string>> GetAllRolesNameForUserIdAsync(Int64 userId, GetOptions? options = null)
            => (await GetAllRolesForUserIdAsync(userId, options)).Select(i => i.Name);

        async Task<bool> UserIdHasAnyRoleAsync(Int64 userId, params string[] checkingRoles)
        {
            var rolesName = (await GetAllRolesNameForUserIdAsync(userId)).ToList();
            return checkingRoles.Any(i => rolesName.Contains(i));
        }

        Task UpdateRolesNameForUserNameAsync(IEnumerable<string> rolesName, string username);

        Task UpdateRolesNameForUserIdAsync(IEnumerable<string> rolesName, Int64 userId);

        async Task<bool> UserIdHasRoleIdAsync(Int64 userId, Int64 roleId)
        {
            var userRole = await GetFirstOrDefaultAsync(new GetOptions
            {
                Filters = {
                    { "UserId", userId },
                    { "RoleId", roleId },
                }
            });

            return userRole != null;
        }
    }
}
