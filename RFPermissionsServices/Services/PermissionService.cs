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
        public async Task<IEnumerable<string>> GetListNameByIdAsync(IEnumerable<long> permissionsId, PermissionQueryOptions? options = null)
        {
            return await permissionRepository.GetListNameByIdAsync(permissionsId, options);
        }
    }
}
