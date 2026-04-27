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
        public async Task<IEnumerable<string>> GetListNamesByIdAsync(IEnumerable<long> ids, RoleQueryOptions? options = null)
        {
            return await roleRepository.GetListNamesByIdAsync(ids, options);
        }
    }
}
