using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions
{
    public class UserNotFoundException()
        : HttpException(400, "User not found.")
    {
    }
}
