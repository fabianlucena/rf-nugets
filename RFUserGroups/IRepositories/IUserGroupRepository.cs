using RFIRepositories.IRepositories;
using RFUserGroups.Entities;
using RFUserGroups.QueryOptions;

namespace RFUserGroups.IRepositories;

public interface IUserGroupRepository : ICommonJoinRepository<UserGroup>
{
    Task<IEnumerable<long>> GetAllGroupsIdByUsersIdAsync(IEnumerable<long> usersId, UserGroupQueryOptions? options = null);
}