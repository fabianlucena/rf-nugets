using RFBaseIServices.IServices;
using RFPermissionsEntities.Entities;
using RFPermissionsEntities.QueryOptions;

namespace RFPermissionsIServices.IServices
{
    public interface IPermissionService : IImmutableEntityService<Permission>
    {
        Task<IEnumerable<string>> GetListNameByIdAsync(IEnumerable<long> permissionsId, PermissionQueryOptions? options = null);
    }
}
