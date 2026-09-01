using RFRBAC.Entities;
using RFRBAC.IServices;
using RFRegisterService.Attributes;
using RFRGOBAC.DTO;
using RFRGOBAC.Entities;
using RFRGOBAC.IRepositories;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;
using RFServices.Services;

namespace RFRGOBAC.Services;

[RegisterService]
public class RoleXUserXOrganizationService(
    IRoleXUserXOrganizationRepository roleXUserXOrganizationRepository,
    IRoleIncludeService roleIncludeService,
    IServiceProvider serviceProvider
) : CommonJoinService<RoleXUserXOrganization>(roleXUserXOrganizationRepository, serviceProvider),
    IRoleXUserXOrganizationService
{
    public async Task<IEnumerable<long>> GetRolesIdByUsersIdAndOrganizationIdAsync(IEnumerable<long> userIds, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null)
    {
        options = options?.Clone() ?? new RoleXUserXOrganizationQueryOptions();
        options.UserIds = userIds;
        options.OrganizationId = OrganizationId;
        return await roleXUserXOrganizationRepository.GetIdsAsync(options);
    }

    public async Task<IEnumerable<long>> GetAllRolesIdByUsersIdAndOrganizationIdAsync(IEnumerable<long> userIds, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null)
    {
        var roleIds = await GetRolesIdByUsersIdAndOrganizationIdAsync(userIds, OrganizationId, options);
        var allRoleIds = await roleIncludeService.GetAllRolesIdByRolesIdAsync(roleIds);
        return allRoleIds;
    }

    public async Task<IEnumerable<Organization>> GetOrganizationsByUsersIdAsync(IEnumerable<long> userIds, RoleXUserXOrganizationQueryOptions? options = null)
    {
        options = options?.Clone() ?? new RoleXUserXOrganizationQueryOptions();
        options.UserIds = userIds;
        return await roleXUserXOrganizationRepository.GetOrganizationsAsync(options);
    }

    public Task<long> SetAllOrganizationsRolesIdForUserIdAsync(IEnumerable<OrganizationRolesId> organizationRolesId, long userId, RoleXUserXOrganizationQueryOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<OrganizationRoles>> GetOrganizationsRolesByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null)
    {
        options = options?.Clone() ?? new RoleXUserXOrganizationQueryOptions();
        options.UserId = userId;
        options.IncludeOrganization = true;
        options.IncludeRole = true;

        var list = await roleXUserXOrganizationRepository.GetListAsync(options);
        var organizationsRoles = new List<OrganizationRoles>();
        foreach (var row in list)
        {
            var organizationRoles = organizationsRoles.FirstOrDefault(or => or.OrganizationId == row.Organization?.Id)
                ?? new OrganizationRoles
                {
                    OrganizationId = row.Organization!.Id,
                    Organization = row.Organization,
                    RolesId = [],
                    Roles = [],
                };

            if (!organizationRoles.RolesId!.Any(id => id == row.Role?.Id))
            {
                ((List<long>)organizationRoles.RolesId!).Add(row.Role!.Id);
                ((List<Role>)organizationRoles.Roles!).Add(row.Role);
            }
        }

        return organizationsRoles;
    }
}
