using Microsoft.Extensions.DependencyInjection;
using RFHttpExceptionsL10n.Middlewares;

namespace RFHttpExceptionsL10n
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFHttpExceptionsL10nServices(this IServiceCollection services)
        {
            services.AddScoped<HttpExceptionL10nMiddleware>();

            return services;
        }
    }
}