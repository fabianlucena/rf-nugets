using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RFHttpExceptions.IExceptions;
using RFL10n;

namespace RFHttpExceptions.Middlewares
{
    public class HttpExceptionMiddleware(
        ILogger<HttpExceptionMiddleware> logger,
        IServiceProvider serviceProvider
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
                    using var scope = serviceProvider.CreateScope();
                    var il10n = serviceProvider.GetService<IL10n>();
                    if (il10n == null)
                    {
                        message = exception.Message;
                    }
                    else
                    {
                        message = await httpException.GetL10nMessage(il10n._);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error while translating exception message.");
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

            await context.Response.WriteAsJsonAsync(new
            {
                Error = errorType,
                message,
            });
        }
    }
}
