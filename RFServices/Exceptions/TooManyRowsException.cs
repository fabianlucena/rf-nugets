using RFBase.Exceptions;

namespace RFServices.Exceptions
{
    public class TooManyRowsException()
        : HttpException(500, $"Query returned too many rows")
    { }
}
