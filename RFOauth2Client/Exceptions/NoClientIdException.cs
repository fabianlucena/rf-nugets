using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoClientIdException(string endpoint)
    : HttpException(400, "No client ID for endpoint {endpoint}.", endpoint)
{
}
