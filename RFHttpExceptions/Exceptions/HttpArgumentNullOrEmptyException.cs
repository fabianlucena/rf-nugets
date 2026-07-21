using RFBase.Exceptions;

namespace RFHttpExceptions.Exceptions;

public class HttpArgumentNullOrEmptyException(string paramName)
    : HttpException(400, "Argument {0} cannot be null or empty", paramName)
{
}