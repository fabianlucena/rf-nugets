using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoUsernameFoundException()
    : HttpException(400, "No username found.")
{
}
