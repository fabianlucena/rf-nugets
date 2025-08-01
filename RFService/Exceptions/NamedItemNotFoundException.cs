using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class NamedItemNotFoundException(string name)
        : HttpException(404, "Item for name \"{0}\" is not found.", name)
    {
    }
}
