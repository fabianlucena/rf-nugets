using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoUserInfoInActionException()
    : HttpException(400, "No user info in action.")
{
}
