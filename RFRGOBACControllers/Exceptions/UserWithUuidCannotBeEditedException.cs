using RFBase.Exceptions;

namespace RFRGOBACControllers.Exceptions;

public class UserWithUuidCannotBeEditedException(Guid uuid)
    : HttpException(403, "User with UUID {0} cannot be edited.", uuid.ToString())
{
}