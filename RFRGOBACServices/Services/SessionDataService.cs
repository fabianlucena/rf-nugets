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
        IRoleXUserXCompanyService roleXUserXCompanyService,
        ISessionCompanyService sessionCompanyService,
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

            sessionData.Companies = await roleXUserXCompanyService
                .GetListCompaniesByUserIdAsync(userId);
            if (!sessionData.Companies.Any())
                return sessionData;

            sessionData.CurrentCompany = await sessionCompanyService.GetSingleOrDefaultCompanyBySessionIdAsync(session.Id);
            if (sessionData.CurrentCompany is null)
            {
                if (sessionData.Companies.Count() != 1)
                    return sessionData;

                sessionData.CurrentCompany = sessionData.Companies.First();
                if (sessionData.CurrentCompany is null)
                    return sessionData;

                await sessionCompanyService.CreateAsync(new SessionCompany {
                    CreatedById = session.UserId,
                    UpdatedById = session.UserId,
                    SessionId = session.Id,
                    CompanyId = sessionData.CurrentCompany.Id,
                });
            }

            sessionData.RolesId = await roleXUserXCompanyService.GetAllRolesIdByUserIdAndCompanyIdAsync(
                userId,
                sessionData.CurrentCompany.Id
            );
            sessionData.RolesNames = await roleService.GetListNamesByIdAsync(sessionData.RolesId);
            sessionData.PermissionsNames = await permissionXRoleService.GetAllPermissionsNamesForRolesIdAsync(sessionData.RolesId);

            return sessionData;
        }
    }
}
