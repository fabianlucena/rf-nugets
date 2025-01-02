using RFHttpExceptions.Exceptions;

namespace RFDapper.Exceptions
{
    public class TooManyRowsException()
        : HttpException(500, $"Query returns too many rows.")
    { }
}
