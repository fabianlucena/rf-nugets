using Microsoft.Extensions.DependencyInjection;
using RFServices.Attributes;
using RFServices.Exceptions;
using RFServices.Interfaces;
using System.Reflection;

namespace RFServices;

public static class SeederExecutor
{
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
            if (!typeof(ISeeder).IsAssignableFrom(seederType))
                throw new SeederMustImplementISeedInitialDataException(seederType.FullName ?? seederType.Name);

            if (seederType.IsGenericType)
                throw new SeederCannotBeAGenericTypeException(seederType.FullName ?? seederType.Name);

            if (seederType.IsAbstract)
                throw new SeederCannotBeAbstractException(seederType.FullName ?? seederType.Name);

            var seeder = (ISeeder)ActivatorUtilities.CreateInstance(provider, seederType);
            await seeder.Run();
        }
    }
}
