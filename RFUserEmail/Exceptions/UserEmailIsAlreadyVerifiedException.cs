using RFHttpExceptions.Exceptions;

namespace RFUserEmail.Exceptions
{
    public class UserEmailIsAlreadyVerifiedException()
        : HttpException(400)
    {
    }
}