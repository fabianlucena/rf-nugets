using RFIRepositories.IRepositories;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IRepositories
{
    public interface IRoleXUserRepository : ICommonJoinRepository<RoleXUser>
    {
        Task<IEnumerable<long>> GetRolesIdAsync(RoleXUserQueryOptions? options = null);
        Task<IEnumerable<string>> GetRolesNameAsync(RoleXUserQueryOptions? options = null);
    }
}