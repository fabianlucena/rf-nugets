using RFAuthEntities.Entities;
using RFBaseIServices.IServices;
using RFRBACIServices.IServices;
using RFRGOBACEntities.Entities;
using RFRGOBACIRepositories.DTO;
using RFRGOBACIServices.IServices;
using RFRGOBACIServices.QueryOptions;
using RFUserGroupsIServices.IServices;

namespace RFRGOBACServices.Services
{
    public class SessionDataService(
        IRoleXUserXOrganizationService roleXUserXOrganizationService,
        IUserGroupService userGroupService,
        IUserService userService,
        ISessionOrganizationService sessionOrganizationService,
        IRoleService roleService,
        IPermissionXRoleService permissionXRoleService
    ) : ISessionDataService
    {
        public async Task<SessionData?> GetSingleOrDefaultBySession(Session session, SessionDataQueryOptions? options = null)
        {
            var userId = session.UserId;
            if (userId <= 0)
                return null;

            var sessionData = new SessionData
            {
                GroupIds = await userGroupService.GetAllGroupIdsByUserIdsAsync([userId]),
            };
            sessionData.GroupNames = await userService.GetUsernamesByIdsAsync(sessionData.GroupIds);

            sessionData.Organizations = await roleXUserXOrganizationService
                .GetListOrganizationsByUserIdsAsync(sessionData.GroupIds);
            if (!sessionData.Organizations.Any())
                return sessionData;

            sessionData.CurrentOrganization = await sessionOrganizationService.GetSingleOrDefaultOrganizationBySessionIdAsync(session.Id);
            if (sessionData.CurrentOrganization is null)
            {
                if (sessionData.Organizations.Count() != 1)
                    return sessionData;

                sessionData.CurrentOrganization = sessionData.Organizations.First();
                if (sessionData.CurrentOrganization is null)
                    return sessionData;

                await sessionOrganizationService.CreateAsync(new SessionOrganization {
                    CreatedById = userId,
                    UpdatedById = userId,
                    SessionId = session.Id,
                    OrganizationId = sessionData.CurrentOrganization.Id,
                });
            }

            sessionData.RoleIds = await roleXUserXOrganizationService.GetAllRoleIdsByUserIdsAndOrganizationIdAsync(
                sessionData.GroupIds,
                sessionData.CurrentOrganization.Id
            );
            sessionData.RoleNames = await roleService.GetNamesByIdsAsync(sessionData.RoleIds);
            sessionData.PermissionNames = await permissionXRoleService.GetPermissionNamesByRoleIdsAsync(sessionData.RoleIds);

            return sessionData;
        }
    }
}
