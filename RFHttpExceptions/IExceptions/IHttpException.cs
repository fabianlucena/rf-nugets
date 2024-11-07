namespace RFHttpExceptions.IExceptions
{
    public interface IHttpException
    {
        int StatusCode { get; }
    }
}
