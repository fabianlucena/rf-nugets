using RFBase.Exceptions;

namespace RFServices.Exceptions
{
    public class NoEntityFoundForUuidException(Guid uuid)
        : HttpException(404, "No entity found matching for uuid {0}.", uuid.ToString())
    { }
}
