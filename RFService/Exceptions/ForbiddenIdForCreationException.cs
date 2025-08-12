using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class ForbiddenIdForCreationException(
        string message = "Providing an ID is not allowed during creation."
    )
        : HttpException(400, message)
    {
    }
}
