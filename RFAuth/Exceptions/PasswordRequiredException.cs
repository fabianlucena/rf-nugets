using RFBase.Exceptions;

namespace RFAuth.Exceptions
{
    public class PasswordRequiredException()
        : HttpException(400, "Password is required.")
    {
    }
}