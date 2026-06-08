using RFBase.Libs;

namespace RFRGOBAC.DTO;

public class ORPGDataResponse
{
    public DataDictionary Data { get; set; } = [];

    public ORPGDataResponse(ORGPData orpgData)
    {
        Data["organizations"] = orpgData.Organizations
            .Select(c => new OrganizationMinDTO(c))
            .ToList();

        if (orpgData.CurrentOrganization is not null)
            Data["currentOrganization"] = new OrganizationMinDTO(orpgData.CurrentOrganization);

        if (orpgData.GroupNames is not null)
            Data["groups"] = orpgData.GroupNames;

        if (orpgData.RoleNames is not null)
            Data["roles"] = orpgData.RoleNames;

        if (orpgData.PermissionNames is not null)
            Data["permissions"] = orpgData.PermissionNames;
    }
}
