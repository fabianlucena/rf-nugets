using RFBaseIRepositories.IRepositories;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;

namespace RFRGOBACIRepositories.IRepositories
{
    public interface IRoleXUserXOrganizationRepository : ICommonJoinRepository<RoleXUserXOrganization>
    {
        Task<IEnumerable<long>> GetListIdByUserIdAndOrganizationIdAsync(long userId, long? OrganizationId, RoleXUserXOrganizationQueryOptions? options = null);
        Task<IEnumerable<Organization>> GetListOrganizationsByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null);
    }
}