using RFHttpExceptions.Exceptions;

namespace RFAuthServices.Exceptions
{
    public class InvalidPasswordException()
        : HttpException(401, "Invalid password.")
    {
    }
}