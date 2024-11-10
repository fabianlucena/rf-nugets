namespace RFHttpExceptions.Exceptions
{
    public class HttpForbiddenException()
        : HttpException(403, $"Forbidden access.")
    {}
}
