using RFHttpExceptions.Exceptions;

namespace RFAuth.Exceptions
{
    public class NoSessionUserDataException()
        : HttpException(401, "No session user data.")
    {
    }
}
