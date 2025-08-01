using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class IdItemNotFoundException(Int64 id)
        : HttpException(404, "Item with the ID \"{0}\" is not found.", id.ToString())
    {
    }
}
