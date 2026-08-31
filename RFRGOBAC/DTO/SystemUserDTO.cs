using RFIServices.DTO;
using RFRBAC.DTO;

namespace RFRGOBAC.DTO;

public class SystemUserDTO(SystemUser user)
    : UserDTO(user)
{
    public IEnumerable<RoleMinDTO>? GlobalRoles { get; set; } = user.GlobalRoles?.Select(r => new RoleMinDTO(r));

    public IEnumerable<OrganizationRolesMinDTO>? OrganizationsRoles { get; set; } = user.OrganizationsRoles?.Select(r => new OrganizationRolesMinDTO(r));
}
