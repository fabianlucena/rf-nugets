using RFIRepositories.IRepositories;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IRepositories
{
    public interface IRoleXUserRepository : ICommonJoinRepository<RoleXUser>
    {
        Task<IEnumerable<long>> GetRoleIdsAsync(RoleXUserQueryOptions? options = null);
        Task<IEnumerable<string>> GetRoleNamesAsync(RoleXUserQueryOptions? options = null);
    }
}