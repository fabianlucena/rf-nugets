using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class UnknownFilterException(string filter)
        : HttpException(400, "Unknown filter: {0}.", filter)
    {
    }
}