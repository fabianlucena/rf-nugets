using RFHttpExceptions.Exceptions;

namespace RFRGOBACEF.Exceptions
{
    public class ErrorCreatingRoleXUserXOrganizationRepositoryException()
        : HttpException(500, "Error creating RoleXUserXOrganization repository.")
    { }
}
