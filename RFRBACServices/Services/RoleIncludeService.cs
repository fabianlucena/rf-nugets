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
        public async Task<IEnumerable<long>> GetAllRoleIdsByRoleIdsAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null)
        {
            return await roleIncludeRepository.GetAllRoleIdsByRoleIdsAsync(roleIds, options);
        }

        public async Task<IEnumerable<string>> GetAllRoleNamesByRoleIdsAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null)
        {
            var allRoleIds = await GetAllRoleIdsByRoleIdsAsync(roleIds, options);
            var allRoleNames = await roleService.GetListNamesByIdAsync(allRoleIds);

            return allRoleNames;
        }
    }
}
