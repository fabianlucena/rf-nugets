using RFBase.Exceptions;

namespace RFAuth.Exceptions
{
    public class InvalidPasswordException()
        : HttpException(401, "Invalid password.")
    {
    }
}