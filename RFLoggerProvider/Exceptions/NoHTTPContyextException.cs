using RFBase.Exceptions;

namespace RFLoggerProvider.Exceptions
{
    public class NoHTTPContyextException()
        : HttpException(500, "No HTTP context")
    {
    }
}