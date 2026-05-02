using RFBaseServices.Services;
using RFPermissionsEntities.Entities;
using RFPermissionsEntities.QueryOptions;
using RFPermissionsIRepositories.Repositories;
using RFPermissionsIServices.IServices;

namespace RFPermissionsServices.Services
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
