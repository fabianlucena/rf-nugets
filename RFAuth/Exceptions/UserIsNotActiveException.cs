using RFBase.Exceptions;

namespace RFAuth.Exceptions
{
    public class UserIsNotActiveException()
        : HttpException(401, "User is not active.")
    {
    }
}