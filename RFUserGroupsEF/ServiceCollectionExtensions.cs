using Microsoft.Extensions.DependencyInjection;
using RFUserGroups.IRepositories;
using RFUserGroupsEF.Repositories;

namespace RFUserGroupsEF;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRFUserGroupsEF(this IServiceCollection services)
    {
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();

        return services;
    }

}