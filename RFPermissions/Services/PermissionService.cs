using RFPermissions.Entities;
using RFPermissions.IRepositories;
using RFPermissions.IServices;
using RFPermissions.QueryOptions;
using RFServices.Services;

namespace RFPermissions.Services
{
    public class PermissionService(IPermissionRepository permissionRepository)
        : ImmutableEntityService<Permission>(permissionRepository),
        IPermissionService
    {
        public async Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, PermissionQueryOptions? options = null)
        {
            options = (PermissionQueryOptions?)(options?.Clone() ?? new PermissionQueryOptions());
            options!.Ids = ids;
            return await permissionRepository.GetNamesAsync(options);
        }
    }
}
