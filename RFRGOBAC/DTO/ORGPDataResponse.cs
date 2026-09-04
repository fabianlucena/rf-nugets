using RFBase.Libs;

namespace RFRGOBAC.DTO;

public class ORPGDataResponse
{
    public DataDictionary Data { get; set; } = [];

    public ORPGDataResponse(ORGPData orpgData)
    {
        Data["organizations"] = orpgData.Organizations
            .Select(c => new OrganizationMinDTO(c));

        if (orpgData.CurrentOrganization is not null)
            Data["currentOrganization"] = new OrganizationMinDTO(orpgData.CurrentOrganization);

        if (orpgData.GroupsName is not null)
            Data["groups"] = orpgData.GroupsName;

        if (orpgData.RolesName is not null)
            Data["roles"] = orpgData.RolesName;

        if (orpgData.PermissionsName is not null)
            Data["permissions"] = orpgData.PermissionsName;
    }
}
