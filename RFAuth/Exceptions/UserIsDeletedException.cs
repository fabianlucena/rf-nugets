using RFBase.Exceptions;

namespace RFAuth.Exceptions
{
    public class UserIsDeletedException()
        : HttpException(401, "User is deleted.")
    {
    }
}