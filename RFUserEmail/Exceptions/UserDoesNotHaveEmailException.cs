using RFHttpExceptions.Exceptions;

namespace RFUserEmail.Exceptions
{
    public class UserDoesNotHaveEmailException()
        : HttpException(404)
    {
    }
}