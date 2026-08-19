using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoURLException(string endpoint)
    : HttpException(400, "No URL for endpoing {endpoint}.", endpoint)
{
}
