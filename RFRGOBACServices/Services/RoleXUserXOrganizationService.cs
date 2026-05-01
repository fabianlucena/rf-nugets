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

        public async Task<IEnumerable<long>> GetRoleIdsByUserIdOrganizationIdAsync(long userId, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null)
        {
            return await roleXUserXOrganizationRepository.GetListIdByUserIdAndOrganizationIdAsync(userId, OrganizationId, options);
        }

        public async Task<IEnumerable<long>> GetAllRoleIdsByUserIdAndOrganizationIdAsync(long userId, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null)
        {
            var roleIds = await GetRoleIdsByUserIdOrganizationIdAsync(userId, OrganizationId, options);
            var allRoleIds = await roleIncludeService.GetAllRoleIdsByRoleIdsAsync(roleIds);
            return allRoleIds;
        }

        public async Task<IEnumerable<Organization>> GetListOrganizationsByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null)
        {
            var OrganizationsList = await roleXUserXOrganizationRepository.GetListOrganizationsByUserIdAsync(userId, options);
            return OrganizationsList;
        }
    }
}
