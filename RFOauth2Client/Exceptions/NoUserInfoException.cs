using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoUserInfoException()
    : HttpException(500, "No user info")
{
}
