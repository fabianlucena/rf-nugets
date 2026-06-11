using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace RFL10n
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFL10nServices(this IServiceCollection services)
        {
            services.AddScoped<IL10n, L10n>();

            return services;
        }
    }
}