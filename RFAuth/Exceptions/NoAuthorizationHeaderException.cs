using RFHttpExceptions.Exceptions;

namespace RFAuth.Exceptions
{
    public class NoAuthorizationHeaderException()
        : HttpException(401, "No authorization header.")
    {
    }
}
