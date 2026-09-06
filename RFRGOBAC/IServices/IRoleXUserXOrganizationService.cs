using RFEntities.Entities;
using RFIServices.IServices;
using RFRBAC.Entities;
using RFRGOBAC.DTO;
using RFRGOBAC.Entities;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.IServices;

public interface IRoleXUserXOrganizationService : ICommonJoinService<RoleXUserXOrganization>
{
    Task<IEnumerable<long>> GetRolesIdAsync(RoleXUserXOrganizationQueryOptions options);
    Task<IEnumerable<long>> GetUsersIdAsync(RoleXUserXOrganizationQueryOptions options);
    Task<IEnumerable<long>> GetOrganizationsIdAsync(RoleXUserXOrganizationQueryOptions options);
    Task<IEnumerable<Role>> GetRolesAsync(RoleXUserXOrganizationQueryOptions options);
    Task<IEnumerable<User>> GetUsersAsync(RoleXUserXOrganizationQueryOptions options);
    Task<IEnumerable<Organization>> GetOrganizationsAsync(RoleXUserXOrganizationQueryOptions options);
    Task<IEnumerable<long>> GetRolesIdByUsersIdAndOrganizationIdAsync(IEnumerable<long> usersId, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<IEnumerable<long>> GetAllRolesIdByUsersIdAndOrganizationIdAsync(IEnumerable<long> usersId, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<IEnumerable<Organization>> GetOrganizationsByUsersIdAsync(IEnumerable<long> usersId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<int> SetOrganizationsRolesIdForUserIdAsync(IEnumerable<OrganizationRolesId> organizationsRolesId, long userId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<IEnumerable<OrganizationRoles>> GetOrganizationsRolesByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null);
    Task<IEnumerable<Organization>> GetOrganizationsByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null);
}
