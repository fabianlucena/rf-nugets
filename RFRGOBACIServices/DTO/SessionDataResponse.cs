using RFBaseEntities.Libs;
using RFRGOBACIRepositories.DTO;

namespace RFRGOBACIServices.DTO
{
    public class SessionDataResponse
    {
        public DataDictionary Data { get; set; } = [];

        public SessionDataResponse(SessionData sessionData)
        {
            Data["companies"] = sessionData.Companies
                .Select(c => new OrganizationMinDTO(c))
                .ToList();

            if (sessionData.CurrentOrganization is not null)
                Data["currentOrganization"] = new OrganizationMinDTO(sessionData.CurrentOrganization);

            if (sessionData.RolesNames is not null)
                Data["roles"] = sessionData.RolesNames;

            if (sessionData.PermissionsNames is not null)
                Data["permissions"] = sessionData.PermissionsNames;
        }
    }
}
