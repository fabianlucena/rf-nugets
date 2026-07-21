using RFHttpExceptions.Exceptions;

namespace RFDapper.Exceptions
{
    public class InvalidObjectBecauseTheIdIsNotALongValueException()
        : HttpException(500, $"Invalid object because the ID is not a long value.")
    { }
}
