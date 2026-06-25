using Microsoft.Extensions.DependencyInjection;
using RFRegisterService.Attributes;
using System.Reflection;

namespace RFRegisterService;

public static class AttributedServiceRegistration
{
    public static IServiceCollection AddAttributedServices(this IServiceCollection services)
    {
        var servicesToRegister = GetServicesToRegister<RegisterServiceAttribute>();
        if (servicesToRegister.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("info: ");
            Console.ResetColor();
            Console.WriteLine("Registering services:");
            foreach (var (ServiceType, ImplementationType, Lifetime) in servicesToRegister)
            {
                Console.WriteLine($"      {ServiceType.Name} -> {ImplementationType.Name}");
                services.Add(new ServiceDescriptor(ServiceType, ImplementationType, Lifetime));
            }
        }

        var decoratorsToRegister = GetServicesToRegister<RegisterDecoratorAttribute>(name => name.EndsWith("Decorator") ? name[..^"Decorator".Length] : name);
        if (decoratorsToRegister.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("info: ");
            Console.ResetColor();
            Console.WriteLine("Registering decorators:");
            foreach (var (ServiceType, DecoratorType, Lifetime) in decoratorsToRegister)
            {
                Console.WriteLine($"      {ServiceType.Name} -> {DecoratorType.Name}");
                services.Decorate(ServiceType, DecoratorType, Lifetime);
            }
        }

        return services;
    }

    public static List<(Type ServiceType, Type ImplementationType, ServiceLifetime Lifetime)> GetServicesToRegister<TAttribute>(
        Func<string, string>? sanitizeName = null
    ) where TAttribute : RegisterServiceAttributeBase
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetCustomAttribute<TAttribute>() != null);

        var servicesToRegister = new List<(Type ServiceType, Type ImplementationType, ServiceLifetime Lifetime)>();
        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<TAttribute>()!;
            var interfaces = attr.Interfaces ?? [];

            if (interfaces.Length == 0)
            {
                var allInterfaces = type.GetInterfaces();
                if (allInterfaces.Length > 0)
                {
                    var name = type.Name;
                    var iName = "I" + name;
                    if (sanitizeName is not null)
                        iName = sanitizeName(iName);

                    interfaces = [.. allInterfaces.Where(i => i.Name == iName)];
                    if (interfaces.Length == 0)
                        interfaces = [.. allInterfaces.Where(i => i.Namespace != null && !i.Namespace.StartsWith("Microsoft."))];
                }

                if (interfaces.Length == 0)
                    interfaces = [type];
            }

            servicesToRegister = [
                ..servicesToRegister,
                ..interfaces.Select(t => (ServiceType: t, ImplementationType: type, attr.Lifetime))
            ];
        }

        return servicesToRegister;
    }
}
