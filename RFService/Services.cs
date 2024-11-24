using Microsoft.Extensions.DependencyInjection;
using RFService.IServices;
using RFService.Services;

namespace RFService
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFService(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBus>();
        }
    }
}