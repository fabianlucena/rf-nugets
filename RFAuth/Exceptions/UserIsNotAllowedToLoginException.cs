using RFBase.Exceptions;

namespace RFAuth.Exceptions
{
    public class UserIsNotAllowedToLoginException()
        : HttpException(401, "User is not allowed to login.")
    {
    }
}