using Microsoft.Extensions.DependencyInjection;
using RFUserGroups.Services;
using RFUserGroupsIServices.IServices;

namespace RFUserGroups;

public static class AddRFUserGroupsServicesRegistration
{
    public static IServiceCollection AddRFUserGroupsServices(this IServiceCollection services)
    {
        services.AddScoped<IUserGroupService, UserGroupService>();

        return services;
    }
}
