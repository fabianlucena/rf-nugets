using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoAccessTokenReceivedException()
    : HttpException(400, "No access token received")
{
}
