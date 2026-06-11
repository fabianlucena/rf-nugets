using RFBase.Exceptions;

namespace RFServices.Exceptions;

public class CreatedByIdMustBeSetForNewEntriesException()
    : HttpException(500, "CreatedById must be set for new entries")
{ }
