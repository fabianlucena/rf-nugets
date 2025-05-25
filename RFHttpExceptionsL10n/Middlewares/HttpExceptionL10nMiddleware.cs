using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RFHttpExceptions.IExceptions;
using RFL10n;

namespace RFHttpExceptionsL10n.Middlewares
{
    public class HttpExceptionL10nMiddleware(
        RequestDelegate next,
        ILogger<HttpExceptionL10nMiddleware> logger,
        IServiceProvider provider
    )
    {
        public async Task InvokeAsync(
            HttpContext context
        )
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

            if (exception is IHttpException httpException)
                context.Response.StatusCode = httpException.StatusCode;
            else
                context.Response.StatusCode = 500;

            var message = exception.Message;
            if (!string.IsNullOrEmpty(message))
            {
                var l10n = provider.GetService<IL10n>();
                if (l10n != null)
                    message = await l10n._("exception", exception.Message);
            }

            var result = new
            {
                Error = exception.GetType().Name,
                Message = message,
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
