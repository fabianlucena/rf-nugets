using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class UuidItemNotFoundException(Guid uuid)
        : HttpException(404, $"Item with UUID {uuid} not found.")
    {
    }
}
