using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RFHttpExceptions.IExceptions;
using RFL10n;

namespace RFHttpExceptionsL10n.Middlewares
{
    public class HttpExceptionL10nMiddleware(
        ILogger<HttpExceptionL10nMiddleware> logger,
        IServiceProvider provider
    )
        : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception
        )
        {
            string message;
            string errorType;
            try
            {
                if (exception is IHttpException httpException)
                {
                    context.Response.StatusCode = httpException.StatusCode;

                try
                {
                    using var scope = provider.CreateScope();
                    var l10n = scope.ServiceProvider.GetService<IL10n>();
                    if (l10n != null)
                    {
                        message = await l10n._c(
                            "exception",
                            httpException.MessageFormat,
                            httpException.Parameters
                        );
                    }
                    else
                    {
                        message = httpException.Message;
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error translating exception message.");
                    message = exception.Message;
                }
            }
            else
            {
                context.Response.StatusCode = 500;
                message = exception.Message;
            }

                errorType = exception.GetType()
                    ?.GetProperty("Error")
                    ?.GetValue(exception)
                    as string
                    ?? exception.GetType().Name;
            }
            catch (Exception)
            {
                errorType = "UnknownError";
                message = "An unexpected error occurred.";
            }

            var result = new
            {
                Error = errorType,
                Message = message,
            };

            try
            {
                logger.LogError(exception, "{type}: {message}", errorType, message);
            }
            catch (Exception) { }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
