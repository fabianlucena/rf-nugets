using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class TooManyRowsException()
        : HttpException(500, $"Query returned too many rows")
    { }
}
