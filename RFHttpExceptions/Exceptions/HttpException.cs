using RFHttpExceptions.IExceptions;

namespace RFHttpExceptions.Exceptions
{
    public class HttpException(int statusCode, string FormatMessage = "", params string[] ParamsList)
        : Exception(), IHttpException
    {
        public int StatusCode { get => statusCode; }
        override public string Message { get => GetMessage(); }

        public static string DoFormatMessage(string formatMessage, params string[] paramsList)
        {
            if (formatMessage == "" || paramsList.Length == 0)
                return formatMessage;
            else
            {
                var message = formatMessage;
                for (int i = 0; i < paramsList.Length; i++)
                    message = message.Replace($"{{{i}}}", paramsList[i]);

                return message;
            }
        }

        public string GetMessage()
            => DoFormatMessage(FormatMessage, ParamsList);
        
        public async Task<string> GetL10nMessage(Func<string, string[], Task<string>> translator)
            => await translator(FormatMessage, ParamsList);
    }
}
