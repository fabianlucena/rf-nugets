using RFHttpExceptions.Exceptions;

namespace RFUserEmailVerified.Exceptions
{
    public class UserDoesNotHaveEmailException()
        : HttpException(404)
    {
    }
}