using RFBaseServices.Services;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;
using RFRBACIRepositories.IRepositories;
using RFRBACIServices.IServices;

namespace RFRBACServices.Services
{
    public class RoleIncludeService(
        IRoleIncludeRepository roleIncludeRepository,
        IRoleService roleService
    )
        : CommonJoinService<RoleInclude>(roleIncludeRepository),
        IRoleIncludeService
    {
        public async Task<IEnumerable<long>> GetAllRolesIdByRolesIdAsync(IEnumerable<long> rolesId, RoleIncludeQueryOptions? options = null)
        {
            return await roleIncludeRepository.GetAllRolesIdByRolesIdAsync(rolesId, options);
        }

        public async Task<IEnumerable<string>> GetAllRolesNamesByRolesIdAsync(IEnumerable<long> rolesId, RoleIncludeQueryOptions? options = null)
        {
            var allRolesId = await GetAllRolesIdByRolesIdAsync(rolesId, options);
            var allRolesNames = await roleService.GetListNamesByIdAsync(allRolesId);

            return allRolesNames;
        }
    }
}
