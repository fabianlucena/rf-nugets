using RFBaseServices.Services;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;
using RFRBACIRepositories.IRepositories;
using RFRBACIServices.IServices;

namespace RFRBACServices.Services
{
    public class RoleService(IRoleRepository roleRepository)
        : CommonEntityService<Role>(roleRepository),
        IRoleService
    {
        public async Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, RoleQueryOptions? options = null)
        {
            options = (RoleQueryOptions?)(options?.Clone() ?? new RoleQueryOptions());
            options!.Ids = ids;
            return await roleRepository.GetNamesAsync(options);
        }
    }
}
