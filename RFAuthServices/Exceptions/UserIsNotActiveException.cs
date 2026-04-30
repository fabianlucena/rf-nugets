using RFHttpExceptions.Exceptions;

namespace RFAuthServices.Exceptions
{
    public class UserIsNotActiveException()
        : HttpException(401, "User is not active.")
    {
    }
}