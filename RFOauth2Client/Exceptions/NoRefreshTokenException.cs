using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoRefreshTokenException()
    : HttpException(400, "No refresh token.")
{
}
