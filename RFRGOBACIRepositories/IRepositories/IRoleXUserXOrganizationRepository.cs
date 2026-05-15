using RFBaseIRepositories.IRepositories;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;

namespace RFRGOBACIRepositories.IRepositories
{
    public interface IRoleXUserXOrganizationRepository : ICommonJoinRepository<RoleXUserXOrganization>
    {
        Task<IEnumerable<long>> GetIdsAsync(RoleXUserXOrganizationQueryOptions options);
        Task<IEnumerable<Organization>> GetOrganizationsAsync(RoleXUserXOrganizationQueryOptions options);
    }
}