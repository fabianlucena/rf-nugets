using RFIServices.IServices;
using RFPermissions.Entities;
using RFPermissions.QueryOptions;

namespace RFPermissions.IServices
{
    public interface IPermissionService : IImmutableEntityService<Permission>
    {
        Task<IEnumerable<string>> GetNamesAsync(PermissionQueryOptions options);
        Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, PermissionQueryOptions? options = null);
        Task<long?> GetSingleIdOrDefaultByNameAsync(string name, PermissionQueryOptions? options = null);
        Task<IEnumerable<long>> GetIdsByNamesAsync(IEnumerable<string> names, PermissionQueryOptions? options = null);
        Task<long> GetIdOrCreateByNameAsync(string name, PermissionQueryOptions? options = null, Func<Permission, Task<Permission>>? createFactory = null);
        Task<IEnumerable<long>> GetIdsOrCreateByNamesAsync(IEnumerable<string> names, PermissionQueryOptions? options = null, Func<Permission, Task<Permission>>? createFactory = null);
    }
}
