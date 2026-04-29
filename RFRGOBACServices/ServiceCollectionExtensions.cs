using Microsoft.Extensions.DependencyInjection;
using RFBaseEntities.Libs;
using RFRGOBACIServices.IServices;
using RFRGOBACServices.Services;

namespace RFRGOBACServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFRGOBACServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleXUserXOrganizationService, RoleXUserXOrganizationService>();
            services.AddScoped<ISessionOrganizationService, SessionOrganizationService>();
            services.AddScoped<ISessionDataService, SessionDataService>();

            var decoratorsBus = DecoratorsBus.Singleton;
            decoratorsBus.Add("LoginResponse", Decorators.LoginResponse);
            decoratorsBus.Add("CheckAutorization", Decorators.CheckAutorization);

            return services;
        }
    }
}
