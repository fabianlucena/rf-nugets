using Microsoft.Extensions.DependencyInjection;
using RFAuthIServices.IServices;
using RFRGOBAC.Decorators;
using RFRGOBACIServices.IServices;
using RFRGOBACServices.Decorators;
using RFRGOBACServices.Services;

namespace RFRGOBAC;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRFRGOBACServices(this IServiceCollection services)
    {
        services.Decorate<ILoginService, LoginServiceDecorator>();
        services.Decorate<ISessionService, SessionServiceDecorator>();

        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IRoleXUserXOrganizationService, RoleXUserXOrganizationService>();
        services.AddScoped<ISessionOrganizationService, SessionOrganizationService>();
        services.AddScoped<ISessionDataService, SessionDataService>();

        return services;
    }
}
