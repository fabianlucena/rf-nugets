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
            logger.LogError(exception, "An unexpected error occurred.");

            context.Response.ContentType = "application/json";

            string message;
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

            string errorType = exception.GetType()
                ?.GetProperty("Error")
                ?.GetValue(exception)
                as string
                ?? exception.GetType().Name;

            var result = new
            {
                Error = errorType,
                Message = message,
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
