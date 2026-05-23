using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;
using RFServices.Services;

namespace RFRBAC.Services
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
