using RFHttpExceptions.Exceptions;

namespace RFAuthServices.Exceptions
{
    public class UserIsDeletedException()
        : HttpException(401, "User is deleted.")
    {
    }
}