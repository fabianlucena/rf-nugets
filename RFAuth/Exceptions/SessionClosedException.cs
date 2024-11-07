using RFService.Exceptions;

namespace RFAuth.Exceptions
{
    public class SessionClosedException() : HttpException(401)
    {
    }
}
