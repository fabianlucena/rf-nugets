using RFRBAC.DTO;

namespace RFRGOBAC.DTO;

public class ORPGDataResponse(ORGPData orpgData)
{
    public OrganizationMinDTO? CurrentOrganization { get; set; } = orpgData.CurrentOrganization is not null ? new OrganizationMinDTO(orpgData.CurrentOrganization) : null;
    public IEnumerable<OrganizationMinDTO> Organizations { get; set; } = orpgData.Organizations.Select(c => new OrganizationMinDTO(c));
    public IEnumerable<string>? Groups{ get; set; } = orpgData.GroupsName;
    public IEnumerable<string>? Roles{ get; set; } = orpgData.RolesName;
    public IEnumerable<string>? Permissions { get; set; } = orpgData.PermissionsName;
}
