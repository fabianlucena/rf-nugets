using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoRedirectURIException(string endpoint)
    : HttpException(400, "No redirect URI for endpoint {endpoint}.", endpoint)
{
}
