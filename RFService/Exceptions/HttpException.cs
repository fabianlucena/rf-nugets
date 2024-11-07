using RFService.IExceptions;

namespace RFService.Exceptions
{
    public class HttpException(int statusCode, string? message = null) : Exception(message), IHttpException
    {
        public int StatusCode { get => statusCode; }
    }
}
