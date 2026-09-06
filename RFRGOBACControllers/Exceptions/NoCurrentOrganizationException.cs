using RFBase.Exceptions;

namespace RFRGOBACControllers.Exceptions;

public class NoCurrentOrganizationException()
    : HttpException(403, "No current organization.")
{
}