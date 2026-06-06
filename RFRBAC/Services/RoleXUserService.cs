using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;
using RFServices.Services;

namespace RFRBAC.Services
{
    public class RoleXUserService(
        IRoleXUserRepository roleXUserRepository,
        IRoleService roleService,
        IRoleIncludeService roleIncludeService
    ) : CommonJoinService<RoleXUser>(roleXUserRepository),
        IRoleXUserService
    {
        public async Task<IEnumerable<long>> GetListRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
            => await roleXUserRepository.GetListRoleIdsByUserIdAsync(userId, options);

        public async Task<IEnumerable<string>> GetListRoleNamesByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
            => await roleXUserRepository.GetListRoleNamesByUserIdAsync(userId, options);

        public async Task<IEnumerable<long>> GetAllRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        {
            var roleIds = await GetListRoleIdsByUserIdAsync(userId, options);
            var allRoleIds = await roleIncludeService.GetAllRoleIdsByRoleIdsAsync(roleIds);
            return allRoleIds;
        }

        public async Task<long> SetAllRolesForUserIdAsync(IEnumerable<string> roles, long userId, RoleXUserQueryOptions? options = null)
        {
            var exisitingRoles = await GetListRoleNamesByUserIdAsync(userId, options);
            var addRoles = roles.Except(exisitingRoles);
            var removeRoles = exisitingRoles.Except(roles);

            var updated = 0;
            foreach (var role in addRoles)
            {
                var roleId = await roleService.GetSingleIdByNameAsync(role);
                await CreateAsync(new RoleXUser
                {
                    UserId = userId,
                    RoleId = roleId
                });
                updated++;
            }

            foreach (var role in removeRoles)
            {
                var roleId = await roleService.GetSingleIdByNameAsync(role);
                await DeleteAsync(new RoleXUserQueryOptions
                {
                    RoleId = roleId,
                    UserId = userId
                });
                updated++;
            }

            return updated;
        }
    }
}
