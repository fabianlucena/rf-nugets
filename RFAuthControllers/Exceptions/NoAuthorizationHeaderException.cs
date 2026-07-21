using RFBase.Exceptions;

namespace RFAuthControllers.Exceptions
{
    public class NoAuthorizationHeaderException()
        : HttpException(401, "No authorization header.")
    {
    }
}
