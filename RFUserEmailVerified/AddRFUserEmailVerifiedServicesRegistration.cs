using Microsoft.Extensions.DependencyInjection;
using RFUserEmailVerified.IServices;
using RFUserEmailVerified.Services;

namespace RFUserEmailVerified;

public static class AddRFUserEmailVerifiedServicesRegistration
{
    public static void AddRFUserEmailVerifiedServices(this IServiceCollection services)
    {
        services.AddScoped<IUserEmailVerifiedService, UserEmailVerifiedService>();
    }
}
