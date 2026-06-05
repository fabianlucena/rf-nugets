using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions
{
    public class AuthorizationErrorException(string message = "Authorization error")
        : HttpException(400, message)
    {
    }
}
