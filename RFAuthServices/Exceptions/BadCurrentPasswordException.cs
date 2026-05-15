using RFHttpExceptions.Exceptions;

namespace RFAuthServices.Exceptions
{
    public class BadCurrentPasswordException()
        : HttpException(401, "Bad current password.")
    {
    }
}