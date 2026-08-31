using RFBase.Exceptions;

namespace RFRGOBACControllers.Exceptions;

public class UserWithUuidNotFoundException(Guid uuid)
    : HttpException(404, "User with UUID = {0} not found", uuid.ToString())
{
}