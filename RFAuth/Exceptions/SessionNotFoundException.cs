using RFBase.Exceptions;

namespace RFAuth.Exceptions
{
    public class SessionNotFoundException()
        : HttpException(401, "Session not found.")
    {
    }
}