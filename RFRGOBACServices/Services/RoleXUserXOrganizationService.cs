using RFBaseServices.Services;
using RFRBACIServices.IServices;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;
using RFRGOBACIRepositories.IRepositories;
using RFRGOBACIServices.IServices;

namespace RFRGOBACServices.Services
{
    public class RoleXUserXOrganizationService(
        IRoleXUserXOrganizationRepository roleXUserXOrganizationRepository,
        IRoleIncludeService roleIncludeService
    ) : CommonJoinService<RoleXUserXOrganization>(roleXUserXOrganizationRepository),
        IRoleXUserXOrganizationService
    {
        public async Task<IEnumerable<long>> GetRoleIdsByUserIdsAndOrganizationIdAsync(IEnumerable<long> userIds, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null)
        {
            options = options?.Clone() ?? new RoleXUserXOrganizationQueryOptions();
            options.UserIds = userIds;
            options.OrganizationId = OrganizationId;
            return await roleXUserXOrganizationRepository.GetIdsAsync(options);
        }

        public async Task<IEnumerable<long>> GetAllRoleIdsByUserIdsAndOrganizationIdAsync(IEnumerable<long> userIds, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null)
        {
            var roleIds = await GetRoleIdsByUserIdsAndOrganizationIdAsync(userIds, OrganizationId, options);
            var allRoleIds = await roleIncludeService.GetAllRoleIdsByRoleIdsAsync(roleIds);
            return allRoleIds;
        }

        public async Task<IEnumerable<Organization>> GetListOrganizationsByUserIdsAsync(IEnumerable<long> userIds, RoleXUserXOrganizationQueryOptions? options = null)
        {
            options = options?.Clone() ?? new RoleXUserXOrganizationQueryOptions();
            options.UserIds = userIds;
            return await roleXUserXOrganizationRepository.GetOrganizationsAsync(options);
        }
    }
}
