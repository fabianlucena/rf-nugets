using RFHttpExceptions.Exceptions;

namespace RFAuthServices.Exceptions
{
    public class UserNotFoundException()
        : HttpException(401, "User not found.")
    {
    }
}