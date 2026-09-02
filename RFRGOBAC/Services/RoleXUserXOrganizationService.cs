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
    public async Task<IEnumerable<long>> GetRolesIdByUsersIdAndOrganizationIdAsync(IEnumerable<long> usersId, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null)
    {
        options = options?.Clone() ?? new RoleXUserXOrganizationQueryOptions();
        options.UsersId = usersId;
        options.OrganizationId = OrganizationId;
        return await roleXUserXOrganizationRepository.GetIdsAsync(options);
    }

    public async Task<IEnumerable<long>> GetAllRolesIdByUsersIdAndOrganizationIdAsync(IEnumerable<long> usersId, long OrganizationId, RoleXUserXOrganizationQueryOptions? options = null)
    {
        var roleIds = await GetRolesIdByUsersIdAndOrganizationIdAsync(usersId, OrganizationId, options);
        var allRoleIds = await roleIncludeService.GetAllRolesIdByRolesIdAsync(roleIds);
        return allRoleIds;
    }

    public async Task<IEnumerable<Organization>> GetOrganizationsByUsersIdAsync(IEnumerable<long> usersId, RoleXUserXOrganizationQueryOptions? options = null)
    {
        options = options?.Clone() ?? new RoleXUserXOrganizationQueryOptions();
        options.UsersId = usersId;
        return await roleXUserXOrganizationRepository.GetOrganizationsAsync(options);
    }

    public async Task<long> SetOrganizationsRolesIdForUserIdAsync(IEnumerable<OrganizationRolesId> organizationsRolesId, long userId, RoleXUserXOrganizationQueryOptions? options = null)
    {
        int result = 0;

        options ??= new RoleXUserXOrganizationQueryOptions();
        options.RoleId = null;
        options.RolesId = null;
        options.OrganizationId = null;
        options.OrganizationsId = null;
        options.UserId = userId;
        options.UsersId = null;
        options.Take = 1000;
        var list = await GetListAsync(options);

        foreach (var organizationRolesId in organizationsRolesId)
        {
            var currentRolesId = list.Where(r => r.OrganizationId == organizationRolesId.OrganizationId).Select(r => r.RoleId);
            var rolesIdToAdd = organizationRolesId.RolesId.Except(currentRolesId);
            var rolesIdToRemove = currentRolesId.Except(organizationRolesId.RolesId);

            if (rolesIdToAdd.Any())
            {
                foreach (var roleId in rolesIdToAdd)
                {
                    await CreateAsync(new RoleXUserXOrganization
                    {
                        UserId = userId,
                        OrganizationId = organizationRolesId.OrganizationId,
                        RoleId = roleId
                    });
                    result++;
                }
            }

            if (rolesIdToRemove.Any())
            {
                result += await DeleteAsync(new RoleXUserXOrganizationQueryOptions
                {
                    UserId = userId,
                    OrganizationId = organizationRolesId.OrganizationId,
                    RolesId = rolesIdToRemove
                });
            }
        }

        var currentOrganizationsId = list.Select(r => r.OrganizationId);
        var organizationsIdToRemove = currentOrganizationsId.Except(organizationsRolesId.Select(or => or.OrganizationId));
        if (organizationsIdToRemove.Any()) {
            result += await DeleteAsync(new RoleXUserXOrganizationQueryOptions
            {
                UserId = userId,
                OrganizationsId = organizationsIdToRemove
            });
        }

        return result;
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
            var organizationRoles = organizationsRoles.FirstOrDefault(or => or.OrganizationId == row.Organization?.Id);
            if (organizationRoles == null)
            {
                organizationRoles = new OrganizationRoles
                {
                    OrganizationId = row.Organization!.Id,
                    Organization = row.Organization,
                    RolesId = [],
                    Roles = [],
                };

                organizationsRoles.Add(organizationRoles);
            }

            if (!organizationRoles.RolesId!.Any(id => id == row.Role?.Id))
            {
                organizationRoles.RolesId = [.. organizationRoles.RolesId, row.Role!.Id];
                organizationRoles.Roles = [.. organizationRoles.Roles!, row.Role];
            }
        }

        return organizationsRoles;
    }

    public async Task<IEnumerable<Organization>> GetOrganizationsByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null)
    {
        options = options?.Clone() ?? new RoleXUserXOrganizationQueryOptions();
        options.UserId = userId;
        options.IncludeOrganization = true;

        var list = await roleXUserXOrganizationRepository.GetListAsync(options);
        
        return list.Select(r => r.Organization).Where(o => o != null)!;
    }
}
