using Microsoft.Extensions.DependencyInjection;
using RFBaseEntities.Libs;
using RFRGCBACIServices.IServices;
using RFRGCBACServices.Services;

namespace RFRGCBACServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFRGCBACServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleXUserXCompanyService, RoleXUserXCompanyService>();
            services.AddScoped<ISessionCompanyService, SessionCompanyService>();
            services.AddScoped<ISessionDataService, SessionDataService>();

            var decoratorsBus = DecoratorsBus.Singleton;
            decoratorsBus.Add("LoginResponse", Decorators.LoginResponse);
            decoratorsBus.Add("CheckAutorization", Decorators.CheckAutorization);

            return services;
        }
    }
}
