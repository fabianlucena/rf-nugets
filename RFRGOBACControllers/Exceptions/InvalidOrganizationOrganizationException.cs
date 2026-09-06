using RFBase.Exceptions;

namespace RFRGOBACControllers.Exceptions;

public class InvalidOrganizationOrganizationException()
    : HttpException(403, "Invalid organization.")
{
}