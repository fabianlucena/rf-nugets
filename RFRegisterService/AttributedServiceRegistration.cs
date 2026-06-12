using Microsoft.Extensions.DependencyInjection;
using RFRegisterService.Attributes;
using System.Reflection;

namespace RFRegisterService;

public static class AttributedServiceRegistration
{
    public static IServiceCollection AddAttributedServices(this IServiceCollection services)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttribute<RegisterServiceAttribute>() != null);

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
}
