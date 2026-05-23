using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;
using RFServices.Services;

namespace RFRBAC.Services
{
    public class RoleXUserService(
        IRoleXUserRepository roleXUserRepository,
        IRoleIncludeService roleIncludeService
    ) : CommonJoinService<RoleXUser>(roleXUserRepository),
        IRoleXUserService
    {
        public async Task<IEnumerable<long>> GetListRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        {
            return await roleXUserRepository.GetListRoleIdsByUserIdAsync(userId, options);
        }

        public async Task<IEnumerable<long>> GetAllRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        {
            var roleIds = await GetListRoleIdsByUserIdAsync(userId, options);
            var allRoleIds = await roleIncludeService.GetAllRoleIdsByRoleIdsAsync(roleIds);
            return allRoleIds;
        }
    }
}
