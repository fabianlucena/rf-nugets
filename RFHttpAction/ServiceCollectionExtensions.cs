using Microsoft.Extensions.DependencyInjection;
using RFHttpAction.IServices;
using RFHttpAction.Services;

namespace RFHttpAction
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFHttpActionServices(this IServiceCollection services)
        {
            services.AddScoped<IHttpActionTypeService, HttpActionTypeService>();
            services.AddScoped<IHttpActionService, HttpActionService>();
            services.AddScoped<IHttpActionListeners, HttpActionListeners>();

            return services;
        }
    }
}
