using RFHttpExceptions.Exceptions;

namespace RFAuth.Exceptions
{
    public class NoSessionUserDataException()
        : HttpException(401)
    {
    }
}
