namespace RFHttpExceptions.IExceptions
{
    public interface IHttpException
    {
        int StatusCode { get; }

        Task<string> GetL10nMessage(Func<string, string[], Task<string>> translator);
    }
}
