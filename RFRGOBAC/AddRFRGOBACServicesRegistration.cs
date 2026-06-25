using Microsoft.Extensions.DependencyInjection;
using RFAuth.IServices;
using RFRGOBAC.Decorators;
using RFRGOBAC.IServices;
using RFRGOBAC.Services;

namespace RFRGOBAC;

public static class AddRFRGOBACServicesRegistration
{
    public static IServiceCollection AddRFRGOBACServices(this IServiceCollection services)
    {
        services.Decorate<ISessionService, SessionServiceDecorator>();

        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IRoleXUserXOrganizationService, RoleXUserXOrganizationService>();
        services.AddScoped<ISessionOrganizationService, SessionOrganizationService>();
        services.AddScoped<IORGPDataService, ORGPDataService>();

        return services;
    }
}
