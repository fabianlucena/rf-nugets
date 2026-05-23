using RFIRepositories.IRepositories;
using RFPermissions.Entities;
using RFPermissions.QueryOptions;

namespace RFPermissions.IRepositories
{
    public interface IPermissionRepository : IImmutableEntityRepository<Permission>
    {
        Task<IEnumerable<string>> GetNamesAsync(PermissionQueryOptions options);
    }
}