using RFIServices.DTO;
using RFRBAC.DTO;

namespace RFRGOBAC.DTO;

public class OrganizationUserDTO(OrganizationUser user)
    : UserDTO(user)
{
    public IEnumerable<RoleMinDTO>? Roles { get; set; } = user.Roles?.Select(r => new RoleMinDTO(r));
}
