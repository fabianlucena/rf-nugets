using RFHttpExceptions.Exceptions;

namespace RFAuth.Exceptions
{
    public class BadCurrentPasswordException()
        : HttpException(401, "Bad current password.")
    {
    }
}