using Microsoft.Extensions.DependencyInjection;
using RFIServices.IServices;
using RFServices.Attributes;
using RFServices.Services;
using System.Reflection;

namespace RFServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRFBaseServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection AddAttributedServices(this IServiceCollection services)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttribute<RegisterServiceAttribute>() != null)
                .Where(t => t.IsClass && !t.IsAbstract);

            var interfacesToRegister = new List<(Type ServiceType, Type ImplementationType, ServiceLifetime Lifetime)>();
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<RegisterServiceAttribute>()!;
                var interfaces = attr.Interfaces ?? [];

                if (interfaces.Length == 0)
                {
                    var name = type.Name;
                    var iName = "I" + name;
                    var allInterfaces = type.GetInterfaces();

                    if (allInterfaces.Length > 0)
                    {
                        interfaces = [.. allInterfaces.Where(i => i.Name == iName)];
                        if (interfaces.Length == 0)
                            interfaces = [..allInterfaces.Where(i => i.Namespace != null && !i.Namespace.StartsWith("Microsoft."))];
                    }

                    if (interfaces.Length == 0)
                        interfaces = [type];
                }

                interfacesToRegister = [
                    ..interfacesToRegister,
                    ..interfaces.Select(t => (ServiceType: t, ImplementationType: type, attr.Lifetime))
                ];
            }

            if (interfacesToRegister.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("info: ");
                Console.ResetColor();
                Console.WriteLine("Registering services:");
                foreach (var (ServiceType, ImplementationType, Lifetime) in interfacesToRegister)
                {
                    Console.WriteLine($"      {ServiceType.Name} -> {ImplementationType.Name}");
                    services.Add(new ServiceDescriptor(ServiceType, ImplementationType, Lifetime));
                }
            }

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
}
