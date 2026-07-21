using Microsoft.Extensions.DependencyInjection;
using RFEntitiesEF.Repositories;
using RFIRepositories.IRepositories;

namespace RFEntitiesEF;

public static class RFEntitiesEFServicesRegistration
{
    public static IServiceCollection RFEntitiesEFServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
