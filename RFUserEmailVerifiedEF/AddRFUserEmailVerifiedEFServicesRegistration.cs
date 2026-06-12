using Microsoft.Extensions.DependencyInjection;
using RFUserEmailVerified.IRepositories;
using RFUserEmailVerifiedEF.Repositories;

namespace RFUserEmailVerifiedEF;

public static class AddRFUserEmailVerifiedEFServicesRegistration
{
    public static IServiceCollection AddRFUserEmailVerifiedEFServices(this IServiceCollection services)
    {
        services.AddScoped<IUserEmailVerifiedRepository, UserEmailVerifiedRepository>();

        return services;
    }
}
