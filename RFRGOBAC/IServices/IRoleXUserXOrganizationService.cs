using RFIServices.IServices;
using RFRGOBAC.DTO;
using RFRGOBAC.Entities;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.IServices;

public interface IRoleXUserXOrganizationService : ICommonJoinService<RoleXUserXOrganization>
{
    Task<IEnumerable<long>> GetRolesIdByUsersIdAndOrganizationIdAsync(IEnumerable<long> userIds, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<IEnumerable<long>> GetAllRolesIdByUsersIdAndOrganizationIdAsync(IEnumerable<long> userIds, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<IEnumerable<Organization>> GetOrganizationsByUsersIdAsync(IEnumerable<long> userIds, RoleXUserXOrganizationQueryOptions? options = null);
    Task<long> SetAllOrganizationsRolesIdForUserIdAsync(IEnumerable<OrganizationRolesId> organizationRolesId, long userId, RoleXUserXOrganizationQueryOptions? options = null);
}
