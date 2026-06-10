using Microsoft.Extensions.DependencyInjection;
using RFIServices.IServices;
using RFServices.Attributes;
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
            .Where(t => t.GetCustomAttribute<SeedDataAttribute>() != null)
            .Where(t => t.IsClass && !t.IsAbstract)
            .OrderBy(t => t.Name) // opcional: orden alfabético
            .ToList();

        foreach (var seederType in seederTypes)
        {
            var seeder = provider.GetRequiredService(seederType);

            var runMethod = seederType.GetMethod("Run")
                ?? throw new InvalidOperationException($"{seederType.Name} no tiene un método Run().");
            var result = runMethod.Invoke(seeder, null);

            if (result is Task task)
                await task;
        }
    }
}
