using RFBase.Exceptions;

namespace RFRGOBACControllers.Exceptions;

public class UserWithUuidIsDeletedException(Guid uuid)
    : HttpException(400, "User with UUID {0} is deleted.", uuid.ToString())
{
}