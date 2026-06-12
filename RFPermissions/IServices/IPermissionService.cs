using RFEntities.Entities;
using RFIServices.IServices;
using RFPermissions.Entities;
using RFPermissions.QueryOptions;

namespace RFPermissions.IServices
{
    public interface IPermissionService : IImmutableEntityService<Permission>
    {
        Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, PermissionQueryOptions? options = null);
        Task<long?> GetSingleIdOrDefaultByNameAsync(string name, PermissionQueryOptions? options = null);
        Task<IEnumerable<long>> GetIdsByNamesAsync(IEnumerable<string> names, PermissionQueryOptions? options = null);
        Task<long> GetSingleIdByNameOrCreateAsync(string name, PermissionQueryOptions? options = null, Func<Permission, Task<Permission>>? createData = null);
        Task<IEnumerable<long>> GetIdsByNamesOrCreateAsync(IEnumerable<string> names, PermissionQueryOptions? options = null, Func<Permission, Task<Permission>>? createData = null);
    }
}
