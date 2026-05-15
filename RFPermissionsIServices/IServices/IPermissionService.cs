using RFBaseIServices.IServices;
using RFPermissionsEntities.Entities;
using RFPermissionsEntities.QueryOptions;

namespace RFPermissionsIServices.IServices
{
    public interface IPermissionService : IImmutableEntityService<Permission>
    {
        Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, PermissionQueryOptions? options = null);
    }
}
