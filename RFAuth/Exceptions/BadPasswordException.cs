using RFService.Exceptions;

namespace RFAuth.Exceptions
{
    public class BadPasswordException() : HttpException(401)
    {
    }
}