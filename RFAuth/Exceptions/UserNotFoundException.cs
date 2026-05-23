using RFBase.Exceptions;

namespace RFAuth.Exceptions
{
    public class UserNotFoundException()
        : HttpException(401, "User not found.")
    {
    }
}