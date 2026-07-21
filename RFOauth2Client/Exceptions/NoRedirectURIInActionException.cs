using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoRedirectURIInActionException()
    : HttpException(400, "No redirect_uri in action.")
{
}
