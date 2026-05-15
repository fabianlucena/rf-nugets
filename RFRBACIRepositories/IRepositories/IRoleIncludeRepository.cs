using RFBaseIRepositories.IRepositories;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIRepositories.IRepositories
{
    public interface IRoleIncludeRepository : ICommonJoinRepository<RoleInclude>
    {
        Task<IEnumerable<long>> GetAllRoleIdsByRoleIdsAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null);
    }
}