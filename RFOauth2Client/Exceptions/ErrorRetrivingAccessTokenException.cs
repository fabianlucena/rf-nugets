using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class ErrorRetrivingAccessTokenException(string message)
    : HttpException(400, "Error retrieving access token: {0}", message)
{
}
