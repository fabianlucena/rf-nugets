using RFHttpExceptions.Exceptions;

namespace RFUserEmailVerified.Exceptions
{
    public class UserEmailIsAlreadyVerifiedException()
        : HttpException(400)
    {
    }
}