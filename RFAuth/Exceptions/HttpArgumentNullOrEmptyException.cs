using RFService.Exceptions;

namespace RFAuth.Exceptions
{
    public class HttpArgumentNullOrEmptyException(string paramName)
        : HttpException(400, $"Argument {paramName} cannot be null or empty")
    {
    }
}