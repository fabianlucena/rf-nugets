using RFAuthEntities.Entities;
using RFAuthIServices.IServices;
using RFRBACIServices.IServices;
using RFRBACServices.Services;
using RFRGOBACEntities.Entities;
using RFRGOBACIRepositories.DTO;
using RFRGOBACIServices.IServices;
using RFRGOBACIServices.QueryOptions;

namespace RFRGOBACServices.Services
{
    public class SessionDataService(
        IRoleXUserXOrganizationService roleXUserXOrganizationService,
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

            var sessionData = new SessionData();

            sessionData.Companies = await roleXUserXOrganizationService
                .GetListCompaniesByUserIdAsync(userId);
            if (!sessionData.Companies.Any())
                return sessionData;

            sessionData.CurrentOrganization = await sessionOrganizationService.GetSingleOrDefaultOrganizationBySessionIdAsync(session.Id);
            if (sessionData.CurrentOrganization is null)
            {
                if (sessionData.Companies.Count() != 1)
                    return sessionData;

                sessionData.CurrentOrganization = sessionData.Companies.First();
                if (sessionData.CurrentOrganization is null)
                    return sessionData;

                await sessionOrganizationService.CreateAsync(new SessionOrganization {
                    CreatedById = session.UserId,
                    UpdatedById = session.UserId,
                    SessionId = session.Id,
                    OrganizationId = sessionData.CurrentOrganization.Id,
                });
            }

            sessionData.RolesId = await roleXUserXOrganizationService.GetAllRolesIdByUserIdAndOrganizationIdAsync(
                userId,
                sessionData.CurrentOrganization.Id
            );
            sessionData.RolesNames = await roleService.GetListNamesByIdAsync(sessionData.RolesId);
            sessionData.PermissionsNames = await permissionXRoleService.GetAllPermissionsNamesForRolesIdAsync(sessionData.RolesId);

            return sessionData;
        }
    }
}
