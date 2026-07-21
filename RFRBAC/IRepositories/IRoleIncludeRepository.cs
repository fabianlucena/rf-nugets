using RFIRepositories.IRepositories;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IRepositories
{
    public interface IRoleIncludeRepository : ICommonJoinRepository<RoleInclude>
    {
        Task<IEnumerable<long>> GetAllRoleIdsByRoleIdsAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null);
    }
}