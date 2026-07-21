using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoTokenURLInActionException()
    : HttpException(400, "No token URL in action.")
{
}
