using RFHttpExceptions.Exceptions;

namespace RFBaseServices.Exceptions
{
    public class PasswordForUserIdNotFoundException(long userId)
        : HttpException(401, "No password found for user ID {0}.", userId.ToString())
    { }
}
