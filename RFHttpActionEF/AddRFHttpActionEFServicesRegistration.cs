using Microsoft.Extensions.DependencyInjection;
using RFHttpAction.IRepositories;
using RFHttpActionEF.Repositories;

namespace RFHttpActionEF;

public static class AddRFHttpActionEFServicesRegistration
{
    public static IServiceCollection AddRFPermissionsEFServices(this IServiceCollection services)
    {
        services.AddScoped<IHttpActionTypeRepository, HttpActionTypeRepository>();
        services.AddScoped<IHttpActionRepository, HttpActionRepository>();

        return services;
    }
}
