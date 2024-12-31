using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RFHttpExceptions.IExceptions;
using RFLocalizer.IServices;

namespace RFHttpExceptionsLocale.Middlewares
{
    public class HttpExceptionLocaleMiddleware(
        RequestDelegate next,
        ILogger<HttpExceptionLocaleMiddleware> logger
    )
    {
        public async Task InvokeAsync(
            HttpContext context,
            ILocalizerFactoryService localizerFactoryService
        )
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception, localizerFactoryService);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            ILocalizerFactoryService localizerFactoryService
        )
        {
            logger.LogError(exception, "An unexpected error occurred.");

            context.Response.ContentType = "application/json";

            if (exception is IHttpException httpException)
                context.Response.StatusCode = httpException.StatusCode;
            else
                context.Response.StatusCode = 500;

            var localizer = localizerFactoryService.GetLocalizerForAcceptLanguage(
                context.Request.Headers.AcceptLanguage,
                "exception"
            );

            var result = new
            {
                Error = exception.GetType().Name,
                Message = localizer[exception.Message],
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
