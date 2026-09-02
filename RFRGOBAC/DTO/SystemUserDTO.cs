using RFIServices.DTO;
using RFRBAC.DTO;

namespace RFRGOBAC.DTO;

public class SystemUserDTO(SystemUser user)
    : UserDTO(user)
{
    public IEnumerable<RoleMinDTO>? SystemRoles { get; set; } = user.SystemRoles?.Select(r => new RoleMinDTO(r));

    public IEnumerable<OrganizationMinDTO>? Organizations { get; set; } = user.Organizations?.Select(o => new OrganizationMinDTO(o));

    public IEnumerable<OrganizationRolesMinDTO>? OrganizationsRoles { get; set; } = user.OrganizationsRoles?.Select(r => new OrganizationRolesMinDTO(r));
}
