using RFService.Exceptions;

namespace RFAuth.Exceptions
{
    public class NoAuthorizationHeaderException() : HttpException(401)
    {
    }
}
