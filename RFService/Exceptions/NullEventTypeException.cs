using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class NullEventTypeException()
        : HttpException(400, "Event type parameter cannot be null or empty.")
    {
    }
}
