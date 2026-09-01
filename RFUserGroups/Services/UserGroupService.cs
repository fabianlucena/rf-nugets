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
    public async Task<IEnumerable<long>> GetAllGroupIdsByUsersIdAsync(IEnumerable<long> usersId, UserGroupQueryOptions? options = null)
        => await userGroupRepository.GetAllGroupsIdByUsersIdAsync(usersId, options);

    public async Task<IEnumerable<string>> GetAllGroupNamesByUsersIdAsync(IEnumerable<long> usersId, UserGroupQueryOptions? options = null)
    {
        var allGroupIds = await GetAllGroupIdsByUsersIdAsync(usersId, options);
        var allGroupNames = await userService.GetUsernamesByIdsAsync(allGroupIds);
        return allGroupNames;
    }
}
