using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace RFL10n
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFL10n(this IServiceCollection services)
        {
            services.AddScoped<IL10n>(provider =>
            {
                var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                var httpContext = httpContextAccessor?.HttpContext;
                if (httpContext != null)
                {
                    var acceptLanguage = httpContext.Request.Headers["Accept-Language"].ToString();
                    if (!string.IsNullOrEmpty(acceptLanguage))
                        return new L10n(provider, acceptLanguage);
                }

                return new L10n(provider, "");
            });
        }
    }
}