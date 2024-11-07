namespace RFService.IExceptions
{
    public interface IHttpException
    {
        int StatusCode { get; }
    }
}
