using RFHttpExceptions.Exceptions;

namespace RFAuth.Exceptions
{
    public class UnknownUserException(string message)
        : HttpException(403, message)
    {
    }
}
