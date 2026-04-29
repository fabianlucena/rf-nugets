using RFBaseIServices.IServices;
using RFRBACIServices.IServices;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;

namespace RFRGOBACIServices.IServices
{
    public interface IRoleXUserXOrganizationService : ICommonJoinService<RoleXUserXOrganization>
    {
        Task<IEnumerable<long>> GetRolesIdByUserIdOrganizationIdAsync(long userId, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null);
        Task<IEnumerable<long>> GetAllRolesIdByUserIdAndOrganizationIdAsync(long userId, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null);
        Task<IEnumerable<Organization>> GetListOrganizationsByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null);
    }
}
