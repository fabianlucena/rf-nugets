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
                .Select(c => new CompanyMinDTO(c))
                .ToList();

            if (sessionData.CurrentCompany is not null)
                Data["currentCompany"] = new CompanyMinDTO(sessionData.CurrentCompany);

            if (sessionData.RolesNames is not null)
                Data["roles"] = sessionData.RolesNames;

            if (sessionData.PermissionsNames is not null)
                Data["permissions"] = sessionData.PermissionsNames;
        }
    }
}
