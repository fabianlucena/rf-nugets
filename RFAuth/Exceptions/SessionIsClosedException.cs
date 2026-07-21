using RFBase.Exceptions;

namespace RFAuth.Exceptions
{
    public class SessionIsClosedException()
        : HttpException(401, "Session is closed.")
    {
    }
}