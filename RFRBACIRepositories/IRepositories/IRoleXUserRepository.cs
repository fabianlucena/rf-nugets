using RFBaseIRepositories.IRepositories;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIRepositories.IRepositories
{
    public interface IRoleXUserRepository : ICommonJoinRepository<RoleXUser>
    {
        Task<IEnumerable<long>> GetListRolesIdByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
    }
}