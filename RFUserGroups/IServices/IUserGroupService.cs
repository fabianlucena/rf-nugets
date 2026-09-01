using RFIServices.IServices;
using RFUserGroups.Entities;
using RFUserGroups.QueryOptions;

namespace RFUserGroupsIServices.IServices
{
    public interface IUserGroupService : ICommonJoinService<UserGroup>
    {
        Task<IEnumerable<long>> GetAllGroupIdsByUsersIdAsync(IEnumerable<long> usersId, UserGroupQueryOptions? options = null);
        Task<IEnumerable<string>> GetAllGroupNamesByUsersIdAsync(IEnumerable<long> usersId, UserGroupQueryOptions? options = null);
    }
}
