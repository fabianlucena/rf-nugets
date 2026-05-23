using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions
{
    public class NoUserInfoInProviderException(string provider)
        : HttpException(404, "No user info in provider '{0}'.", provider)
    {
    }
}
