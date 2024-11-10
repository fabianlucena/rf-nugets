using Microsoft.Extensions.DependencyInjection;
using RFRGBAC.IServices;
using RFRGBAC.Services;

namespace RFRGBAC
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFRBAC(this IServiceCollection services)
        {
            services.AddScoped<IUserGroupService, UserGroupService>();
        }
    }
}