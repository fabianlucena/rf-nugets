using RFHttpExceptions.Exceptions;

namespace RFAuth.Exceptions
{
    public class ClosingSessionException()
        : HttpException(500, "Closing session error")
    {
    }
}
