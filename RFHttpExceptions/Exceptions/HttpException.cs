using RFHttpExceptions.IExceptions;

namespace RFHttpExceptions.Exceptions
{
    public class HttpException(int statusCode, string FormatMessage = "", params string[] ParamsList)
        : Exception(), IHttpException
    {
        public int StatusCode { get => statusCode; }
        override public string Message { get => GetMessage(); }

        public string GetMessage()
        {
            if (FormatMessage == "" || ParamsList.Length == 0)
                return FormatMessage;
            else
            {
                var message = FormatMessage;
                for (int i = 0; i < ParamsList.Length; i++)
                    message = message.Replace($"{{{i}}}", ParamsList[i]);
                return message;
            }
        }
    }
}
