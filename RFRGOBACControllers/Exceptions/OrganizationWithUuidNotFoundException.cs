using RFBase.Exceptions;

namespace RFRGOBACControllers.Exceptions;

public class OrganizationWithUuidNotFoundException(Guid uuid)
    : HttpException(404, "Organization with UUID = {0} not found", uuid.ToString())
{
}