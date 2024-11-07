using RFHttpExceptions.IExceptions;

namespace RFHttpExceptions.Exceptions
{
    public class HttpException(int statusCode, string? message = null) : Exception(message), IHttpException
    {
        public int StatusCode { get => statusCode; }
    }
}
