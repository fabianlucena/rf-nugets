using RFHttpExceptions.Exceptions;

namespace RFAuthServices.Exceptions
{
    public class SessionNotFoundException()
        : HttpException(401, "Session not found.")
    {
    }
}