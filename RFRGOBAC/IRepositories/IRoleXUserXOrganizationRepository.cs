using RFIRepositories.IRepositories;
using RFRGOBAC.Entities;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.IRepositories;

public interface IRoleXUserXOrganizationRepository : ICommonJoinRepository<RoleXUserXOrganization>
{
    Task<IEnumerable<long>> GetIdsAsync(RoleXUserXOrganizationQueryOptions options);
    Task<IEnumerable<Organization>> GetOrganizationsAsync(RoleXUserXOrganizationQueryOptions options);
}