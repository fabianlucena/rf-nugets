using RFBase.Exceptions;

namespace RFServices.Exceptions
{
    public class UpdatedByIdMustBeSetForAuditableEntriesException()
        : HttpException(500, "UpdatedById must be set for auditable entries")
    { }
}
