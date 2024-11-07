using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RFHttpExceptions.IExceptions;

namespace RFHttpExceptions.Middlewares
{
    public class HttpExceptionMiddleware(RequestDelegate next, ILogger<HttpExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            logger.LogError(exception, "An unexpected error occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (exception as IHttpException)?.StatusCode ?? 500;
            await context.Response.WriteAsJsonAsync(new
            {
                Error = exception.GetType().Name,
                exception.Message,
            });
        }
    }
}
