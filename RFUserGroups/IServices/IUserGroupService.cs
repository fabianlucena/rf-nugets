using RFIServices.IServices;
using RFUserGroups.Entities;
using RFUserGroups.QueryOptions;

namespace RFUserGroupsIServices.IServices
{
    public interface IUserGroupService : ICommonJoinService<UserGroup>
    {
        Task<IEnumerable<long>> GetAllGroupIdsByUserIdsAsync(IEnumerable<long> userIds, UserGroupQueryOptions? options = null);
        Task<IEnumerable<string>> GetAllGroupNamesByUserIdsAsync(IEnumerable<long> userIds, UserGroupQueryOptions? options = null);
    }
}
