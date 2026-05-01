using RFBaseEntities.Libs;
using RFRGOBACIRepositories.DTO;

namespace RFRGOBACIServices.DTO
{
    public class SessionDataResponse
    {
        public DataDictionary Data { get; set; } = [];

        public SessionDataResponse(SessionData sessionData)
        {
            Data["organizations"] = sessionData.Organizations
                .Select(c => new OrganizationMinDTO(c))
                .ToList();

            if (sessionData.CurrentOrganization is not null)
                Data["currentOrganization"] = new OrganizationMinDTO(sessionData.CurrentOrganization);

            if (sessionData.RoleNames is not null)
                Data["roles"] = sessionData.RoleNames;

            if (sessionData.PermissionNames is not null)
                Data["permissions"] = sessionData.PermissionNames;
        }
    }
}
