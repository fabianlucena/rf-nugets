using RFEntities.Entities;
using RFRGOBAC.DTO;

namespace RFRGOBACControllers.DTO;

public class OrganizationUserResponse(User user) : OrganizationUserDTO(user)
{
}
