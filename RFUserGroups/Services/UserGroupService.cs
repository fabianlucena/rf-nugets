using RFIServices.IServices;
using RFRegisterService.Attributes;
using RFServices.Services;
using RFUserGroups.Entities;
using RFUserGroups.IRepositories;
using RFUserGroups.QueryOptions;
using RFUserGroupsIServices.IServices;

namespace RFUserGroups.Services;

[RegisterService]
public class UserGroupService(
    IUserGroupRepository userGroupRepository,
    IUserService userService,
    IServiceProvider serviceProvider
)
    : CommonJoinService<UserGroup>(userGroupRepository, serviceProvider),
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
