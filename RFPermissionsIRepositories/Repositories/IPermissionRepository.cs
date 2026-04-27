using RFBaseIRepositories.IRepositories;
using RFPermissionsEntities.Entities;
using RFPermissionsEntities.QueryOptions;

namespace RFPermissionsIRepositories.Repositories
{
    public interface IPermissionRepository : IImmutableEntityRepository<Permission>
    {
        Task<IEnumerable<string>> GetListNameByIdAsync(IEnumerable<long> permissionsId, PermissionQueryOptions? options = null);
    }
}