using RFAuth.Entities;
using RFIServices.IServices;
using RFRBAC.IServices;
using RFRegisterService.Attributes;
using RFRGOBAC.DTO;
using RFRGOBAC.Entities;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;
using RFUserGroupsIServices.IServices;

namespace RFRGOBAC.Services;

[RegisterService]
public class ORGPDataService(
    IRoleXUserXOrganizationService roleXUserXOrganizationService,
    IUserGroupService userGroupService,
    IUserService userService,
    ISessionOrganizationService sessionOrganizationService,
    IRoleService roleService,
    IPermissionXRoleService permissionXRoleService
) : IORGPDataService
{
    public async Task<ORGPData?> GetSingleOrDefaultBySession(Session session, ORGPDataQueryOptions? options = null)
    {
        var userId = session.UserId;
        if (userId <= 0)
            return null;

        var orpgData = new ORGPData
        {
            GroupsId = await userGroupService.GetAllGroupsIdByUsersIdAsync([userId]),
        };
        orpgData.GroupsName = await userService.GetUsernamesByIdsAsync(orpgData.GroupsId);

        orpgData.Organizations = (await roleXUserXOrganizationService
            .GetOrganizationsByUsersIdAsync(orpgData.GroupsId))
            .DistinctBy(o => o.Id);
        if (!orpgData.Organizations.Any())
            return orpgData;

        orpgData.CurrentOrganization = await sessionOrganizationService.GetSingleOrDefaultOrganizationBySessionIdAsync(session.Id);
        if (orpgData.CurrentOrganization is null)
        {
            if (orpgData.Organizations.Count() != 1)
                return orpgData;

            orpgData.CurrentOrganization = orpgData.Organizations.First();
            if (orpgData.CurrentOrganization is null)
                return orpgData;

            await sessionOrganizationService.CreateAsync(new SessionOrganization {
                CreatedById = userId,
                UpdatedById = userId,
                SessionId = session.Id,
                OrganizationId = orpgData.CurrentOrganization.Id,
            });
        }

        orpgData.RolesId = await roleXUserXOrganizationService.GetAllRolesIdByUsersIdAndOrganizationIdAsync(
            orpgData.GroupsId,
            orpgData.CurrentOrganization.Id
        );
        orpgData.RolesName = await roleService.GetNamesByIdsAsync(orpgData.RolesId);
        orpgData.PermissionsName = await permissionXRoleService.GetPermissionsNameByRolesIdAsync(orpgData.RolesId);

        return orpgData;
    }
}
