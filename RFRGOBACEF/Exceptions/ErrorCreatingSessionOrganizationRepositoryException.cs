using RFHttpExceptions.Exceptions;

namespace RFRGOBACEF.Exceptions
{
    public class ErrorCreatingSessionOrganizationRepositoryException()
        : HttpException(500, "Error creating SessionOrganizationRepository")
    { }
}
