using Microsoft.Extensions.DependencyInjection;
using RFService.IServices;
using RFService.Libs;

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