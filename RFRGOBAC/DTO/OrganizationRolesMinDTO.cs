using RFRBAC.DTO;

namespace RFRGOBAC.DTO;

public class OrganizationRolesMinDTO(OrganizationRoles organizationRoles)
{
    public OrganizationMinDTO? Organization { get; set; } = organizationRoles.Organization != null ? new OrganizationMinDTO(organizationRoles.Organization) : null;

    public IEnumerable<RoleMinDTO>? Roles { get; set; } = organizationRoles.Roles?.Select(r => new RoleMinDTO(r));
}
