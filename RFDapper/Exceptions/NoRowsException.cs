using RFHttpExceptions.Exceptions;

namespace RFDapper.Exceptions
{
    public class NoRowsException()
        : HttpException(500, $"Query returns no rows.")
    { }
}
