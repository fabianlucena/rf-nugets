using RFBaseIRepositories.IRepositories;
using RFUserGroupsEntities.Entities;
using RFUserGroupsEntities.QueryOptions;

namespace RFUserGroupsIRepositories.IRepositories
{
    public interface IUserGroupRepository : ICommonJoinRepository<UserGroup>
    {
        Task<IEnumerable<long>> GetAllGroupIdsByUserIdsAsync(IEnumerable<long> userIds, UserGroupQueryOptions? options = null);
    }
}