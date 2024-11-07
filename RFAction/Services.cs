using Microsoft.Extensions.DependencyInjection;
using RFHttpAction.IServices;
using RFHttpAction.Services;

namespace RFHttpAction
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFHttpAction(this IServiceCollection services)
        {
            services.AddScoped<IHttpActionTypeService, HttpActionTypeService>();
            services.AddScoped<IHttpActionService, HttpActionService>();
            services.AddScoped<IHttpActionListeners, HttpActionListeners>();
        }
    }
}
