using RFHttpExceptions.Exceptions;

namespace RFAuthServices.Exceptions
{
    public class UserIsNotAllowedToLoginException()
        : HttpException(401, "User is not allowed to login.")
    {
    }
}