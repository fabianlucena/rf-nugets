using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class IdItemNotFoundException(Int64 id)
        : HttpException(404, $"Item with ID {id} not found.")
    {
    }
}
