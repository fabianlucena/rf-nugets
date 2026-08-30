using RFEntities.Entities;
using RFIServices.DTO;

namespace RFRGOBAC.DTO;

public class OrganizationUserDTO(User user)
    : UserDTO(user)
{
}
