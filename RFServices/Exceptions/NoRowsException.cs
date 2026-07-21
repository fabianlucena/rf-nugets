using RFBase.Exceptions;

namespace RFServices.Exceptions
{
    public class NoRowsException()
        : HttpException(500, $"Query returned no rows")
    { }
}
