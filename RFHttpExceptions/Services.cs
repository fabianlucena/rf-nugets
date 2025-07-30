using Microsoft.Extensions.DependencyInjection;
using RFHttpExceptions.Middlewares;

namespace RFHttpExceptions
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFHttpExceptions(this IServiceCollection services)
        {
            services.AddScoped<HttpExceptionMiddleware>();
        }
    }
}