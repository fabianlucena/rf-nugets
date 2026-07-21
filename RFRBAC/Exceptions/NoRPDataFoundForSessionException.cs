using RFBase.Exceptions;

namespace RFRBAC.Exceptions;

public class NoRPDataFoundForSessionException(long sessionId)
    : HttpException(404, "No RPData found for session ID {0}", sessionId.ToString())
{
}