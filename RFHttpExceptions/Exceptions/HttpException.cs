using RFHttpExceptions.IExceptions;

namespace RFHttpExceptions.Exceptions
{
    public class HttpException(int statusCode, string messageFormat = "", params string[] parameters)
        : Exception(), IHttpException
    {
        public int StatusCode { get => statusCode; }
        public string MessageFormat { get => messageFormat; }
        public string[] Parameters{ get => parameters; }
        override public string Message { get => GetMessage(); }

        public string FormatMessage(string formatMessage, params string[] paramsList)
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
            => FormatMessage(MessageFormat, Parameters);
    }
}
