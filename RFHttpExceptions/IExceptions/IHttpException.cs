namespace RFHttpExceptions.IExceptions
{
    public interface IHttpException
    {
        int StatusCode { get; }
        string Message { get; }
        string MessageFormat { get; }
        string[] Parameters { get; }

        string FormatMessage(string formatMessage, params string[] parameters);
    }
}
