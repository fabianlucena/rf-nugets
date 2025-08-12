using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class UuidItemNotFoundException(Guid uuid)
        : HttpException(404, "Item for UUID \"{0}\" is not found.", uuid.ToString())
    {
    }
}
