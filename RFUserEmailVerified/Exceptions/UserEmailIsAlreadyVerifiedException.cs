using RFHttpExceptions.Exceptions;

namespace RFUserEmailVerified.Exceptions
{
    public class UserEmailIsAlreadyVerifiedException()
        : HttpException(400, "User email is already verified")
    {
    }
}