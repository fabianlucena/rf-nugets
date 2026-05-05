using RFBaseIServices.IServices;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;

namespace RFRGOBACIServices.IServices
{
    public interface IRoleXUserXOrganizationService : ICommonJoinService<RoleXUserXOrganization>
    {
        Task<IEnumerable<long>> GetRoleIdsByUserIdsAndOrganizationIdAsync(IEnumerable<long> userIds, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null);
        Task<IEnumerable<long>> GetAllRoleIdsByUserIdsAndOrganizationIdAsync(IEnumerable<long> userIds, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null);
        Task<IEnumerable<Organization>> GetListOrganizationsByUserIdsAsync(IEnumerable<long> userIds, RoleXUserXOrganizationQueryOptions? options = null);
    }
}
