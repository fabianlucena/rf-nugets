using RFBase.Exceptions;

namespace RFRBAC.Exceptions;

public class RoleWithUuidNotFoundException(Guid uuid)
    : HttpException(404, "Role with UUID = {0} not found", uuid.ToString())
{
}