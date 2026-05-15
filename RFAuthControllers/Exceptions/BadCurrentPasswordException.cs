using RFHttpExceptions.Exceptions;

namespace RFAuthControllers.Exceptions
{
    public class BadCurrentPasswordException()
        : HttpException(401, "Bad current password.")
    {
    }
}