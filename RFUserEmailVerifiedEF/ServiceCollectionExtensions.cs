using Microsoft.Extensions.DependencyInjection;
using RFUserEmailVerified.IRepositories;
using RFUserEmailVerifiedEF.Repositories;

namespace RFUserEmailVerifiedEF;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRFUserEmailVerifiedEF(this IServiceCollection services)
    {
        services.AddScoped<IUserEmailVerifiedRepository, UserEmailVerifiedRepository>();

        return services;
    }
}
