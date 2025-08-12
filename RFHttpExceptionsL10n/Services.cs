using Microsoft.Extensions.DependencyInjection;
using RFHttpExceptionsL10n.Middlewares;

namespace RFHttpExceptionsL10n
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFHttpExceptionsL10n(this IServiceCollection services)
        {
            services.AddScoped<HttpExceptionL10nMiddleware>();
        }
    }
}