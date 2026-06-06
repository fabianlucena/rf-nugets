using Microsoft.Extensions.DependencyInjection;
using RFUserEmailVerified.IServices;
using RFUserEmailVerified.Services;

namespace RFUserEmailVerified
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRFUserEmailVerified(this IServiceCollection services)
        {
            services.AddScoped<IUserEmailVerifiedService, UserEmailVerifiedService>();
        }
    }
}
