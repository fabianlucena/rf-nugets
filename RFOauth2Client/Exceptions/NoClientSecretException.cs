using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoClientSecretException(string endpoint)
    : HttpException(400, "No client secret for endpoint {endpoint}.", endpoint)
{
}
