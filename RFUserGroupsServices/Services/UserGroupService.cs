using RFBaseIServices.IServices;
using RFBaseServices.Services;
using RFUserGroupsEntities.Entities;
using RFUserGroupsEntities.QueryOptions;
using RFUserGroupsIRepositories.IRepositories;
using RFUserGroupsIServices.IServices;

namespace RFUserGroupsServices.Services
{
    public class UserGroupService(
        IUserGroupRepository userGroupRepository,
        IUserService userService
    )
        : CommonJoinService<UserGroup>(userGroupRepository),
        IUserGroupService
    {
        public async Task<IEnumerable<long>> GetAllGroupIdsByUserIdsAsync(IEnumerable<long> userIds, UserGroupQueryOptions? options = null)
            => await userGroupRepository.GetAllGroupIdsByUserIdsAsync(userIds, options);

        public async Task<IEnumerable<string>> GetAllGroupNamesByUserIdsAsync(IEnumerable<long> userIds, UserGroupQueryOptions? options = null)
        {
            var allGroupIds = await GetAllGroupIdsByUserIdsAsync(userIds, options);
            var allGroupNames = await userService.GetUsernamesByIdsAsync(allGroupIds);
            return allGroupNames;
        }
    }
}
