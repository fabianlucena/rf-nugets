using RFIRepositories.IRepositories;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IRepositories
{
    public interface IRoleXUserRepository : ICommonJoinRepository<RoleXUser>
    {
        Task<IEnumerable<long>> GetListRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<string>> GetListRoleNamesByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
    }
}