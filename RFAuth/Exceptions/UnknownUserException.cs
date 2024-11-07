using RFService.Exceptions;

namespace RFAuth.Exceptions
{
    public class UnknownUserException() : HttpException(403)
    {
    }
}
