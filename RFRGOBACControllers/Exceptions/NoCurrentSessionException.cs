using RFBase.Exceptions;

namespace RFRGOBACControllers.Exceptions;

public class NoCurrentSessionException()
    : HttpException(404, "No current session.")
{
}