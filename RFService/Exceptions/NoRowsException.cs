using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class NoRowsException()
        : HttpException(500, $"Query returned no rows")
    { }
}
