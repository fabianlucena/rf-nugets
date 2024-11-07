using RFService.Exceptions;

namespace RFAuth.Exceptions
{
    public class BadAuthorizationSchemeException() : HttpException(401)
    {
    }
}