using Microsoft.Extensions.DependencyInjection;
using RFUserGroupsEF.Repositories;
using RFUserGroupsIRepositories.IRepositories;

namespace RFUserGroupsEF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFUserGroupsEF(this IServiceCollection services)
        {
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();

            return services;
        }

    }
}
