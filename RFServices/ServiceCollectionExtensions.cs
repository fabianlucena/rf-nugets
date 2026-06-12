using Microsoft.Extensions.DependencyInjection;
using RFIServices.IServices;
using RFServices.Attributes;
using RFServices.Exceptions;
using RFServices.Interfaces;
using RFServices.Services;
using System.Reflection;

namespace RFServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRFBaseServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static async Task ExecuteSeedersAsync(this IServiceProvider provider)
    {
        var seederTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && t.GetCustomAttribute<SeedDataAttribute>() != null)
            .ToList();

        if (seederTypes.Count == 0)
            return;

        foreach (var seederType in seederTypes)
        {
            if (!typeof(ISeedInitialData).IsAssignableFrom(seederType))
                throw new SeederMustImplementISeedInitialDataException(seederType.Name);

            if (seederType.IsGenericType)
                throw new SeederCannotBeAGenericTypeException(seederType.Name);

            if (seederType.IsAbstract)
                throw new SeederCannotBeAbstractException($"{seederType.Name} no puede ser abstracta.");

            var seeder = (ISeedInitialData)ActivatorUtilities.CreateInstance(provider, seederType);
            await seeder.Run();
        }
    }
}
