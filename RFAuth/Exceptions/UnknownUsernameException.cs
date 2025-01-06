using RFHttpExceptions.Exceptions;

namespace RFAuth.Exceptions
{
    public class UnknownUsernameException()
        : HttpException(403, "Unknown username.")
    {
    }
}
