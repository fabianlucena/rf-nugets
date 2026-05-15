using RFHttpExceptions.Exceptions;

namespace RFAuthServices.Exceptions
{
    public class SessionIsClosedException()
        : HttpException(401, "Session is closed.")
    {
    }
}