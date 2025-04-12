using RFHttpExceptions.Exceptions;

namespace RFAuth.Exceptions
{
    public class BadAuthorizationSchemeException()
        : HttpException(401, "Bad authorization scheme.")
    {
    }
}