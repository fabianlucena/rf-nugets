using RFHttpExceptions.Exceptions;

namespace RFDapper.Exceptions
{
    public class TooManyJoinsException()
        : HttpException(500, $"There are too many joins.")
    { }
}
