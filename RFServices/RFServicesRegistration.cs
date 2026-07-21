using Microsoft.Extensions.DependencyInjection;
using RFIServices.IServices;
using RFServices.Services;

namespace RFServices;

public static class RFServicesRegistration
{
    public static IServiceCollection AddRFServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
