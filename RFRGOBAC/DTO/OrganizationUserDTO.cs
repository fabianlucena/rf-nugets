using RFEntities.Entities;
using RFIServices.DTO;

namespace RFRGOBAC.DTO;

public class OrganizationUserDTO(OrganizationUser user)
    : UserDTO(user)
{
}
