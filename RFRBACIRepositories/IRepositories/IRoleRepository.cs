using RFBaseIRepositories.IRepositories;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIRepositories.IRepositories
{
    public interface IRoleRepository : ICommonEntityRepository<Role>
    {
        Task<IEnumerable<string>> GetListNamesByIdAsync(IEnumerable<long> ids, RoleQueryOptions? options = null);
    }
}