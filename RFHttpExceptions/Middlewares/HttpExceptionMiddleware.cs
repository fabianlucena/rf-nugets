using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RFBase.IExceptions;
using System.Text.Json;

namespace RFHttpExceptions.Middlewares;

public class HttpExceptionMiddleware(
    RequestDelegate next,
    ILogger<HttpExceptionMiddleware> logger
)
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
                message = exception.Message;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting exception message.");
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

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
}
