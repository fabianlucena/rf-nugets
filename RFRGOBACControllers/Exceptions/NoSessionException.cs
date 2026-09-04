using RFBase.Exceptions;

namespace RFRGOBACControllers.Exceptions;

public class NoSessionException()
    : HttpException(404, "No session.")
{
}