using Microsoft.Extensions.DependencyInjection;
using RFUserGroupsIServices.IServices;
using RFUserGroupsServices.Services;

namespace RFUserGroupsServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFUserGroupsServices(this IServiceCollection services)
        {
            services.AddScoped<IUserGroupService, UserGroupService>();

            return services;
        }
    }
}
