using Microsoft.Extensions.DependencyInjection;
using RFBaseEntities.ILibs;
using RFBaseEntities.Libs;

namespace RFBaseEntities
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFBaseEntitiesServices(this IServiceCollection services)
        {
            services.AddSingleton<IDecoratorsBus>(DecoratorsBus.Singleton);

            return services;
        }

    }
}
