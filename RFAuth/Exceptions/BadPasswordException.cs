using RFHttpExceptions.Exceptions;

namespace RFAuth.Exceptions
{
    public class BadPasswordException(string message)
        : HttpException(401, message)
    {
    }
}