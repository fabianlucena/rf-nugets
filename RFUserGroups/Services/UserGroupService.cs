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
    public async Task<IEnumerable<long>> GetAllGroupsIdByUsersIdAsync(IEnumerable<long> usersId, UserGroupQueryOptions? options = null)
        => await userGroupRepository.GetAllGroupsIdByUsersIdAsync(usersId, options);

    public async Task<IEnumerable<string>> GetAllGroupsNameByUsersIdAsync(IEnumerable<long> usersId, UserGroupQueryOptions? options = null)
    {
        var allGroupsId = await GetAllGroupsIdByUsersIdAsync(usersId, options);
        var allGroupsName = await userService.GetUsernamesByIdsAsync(allGroupsId);
        return allGroupsName;
    }
}
