using RFHttpExceptions.Exceptions;

namespace RFAuth.Exceptions
{
    public class InvalidCredentialsException()
        : HttpException(403, "Invalid credentials.")
    {
    }
}
