using RFHttpExceptions.Exceptions;

namespace RFDapper.Exceptions
{
    public class NoEntityForJoinException()
        : HttpException(500, $"No On clause for join with entity.")
    { }
}
