using RFIServices.IServices;
using RFRGOBAC.DTO;
using RFRGOBAC.Entities;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.IServices;

public interface IRoleXUserXOrganizationService : ICommonJoinService<RoleXUserXOrganization>
{
    Task<IEnumerable<long>> GetRolesIdByUsersIdAndOrganizationIdAsync(IEnumerable<long> usersId, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<IEnumerable<long>> GetAllRolesIdByUsersIdAndOrganizationIdAsync(IEnumerable<long> usersId, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<IEnumerable<Organization>> GetOrganizationsByUsersIdAsync(IEnumerable<long> usersId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<long> SetOrganizationsRolesIdForUserIdAsync(IEnumerable<OrganizationRolesId> organizationsRolesId, long userId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<IEnumerable<OrganizationRoles>> GetOrganizationsRolesByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<IEnumerable<Organization>> GetOrganizationsByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null);
}
