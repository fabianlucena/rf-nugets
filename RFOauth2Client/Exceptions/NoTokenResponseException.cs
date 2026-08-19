using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoTokenResponseException()
    : HttpException(400, "No token response.")
{
}
