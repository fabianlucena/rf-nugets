using RFIServices.IServices;
using RFPermissions.Entities;
using RFPermissions.QueryOptions;

namespace RFPermissions.IServices
{
    public interface IPermissionService : IImmutableEntityService<Permission>
    {
        Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, PermissionQueryOptions? options = null);
    }
}
