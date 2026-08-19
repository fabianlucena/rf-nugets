using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace RFEventBus;

public static class WebApplicationExtensions
{
    public static void SetupEventBus(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, MemoryEventBus>();
        SetupEventHandlers(services);
    }

    public static void SetupEventHandlers(this IServiceCollection services)
    {
        var methods = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => {
                try { return a.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null); }
            })
            .SelectMany(t => t!.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            .Where(m => m.IsDefined(typeof(EventHandlerAttribute), false));

        if (!methods.Any())
            return;

        var provider = services.BuildServiceProvider();
        var eventBus = provider.GetRequiredService<IEventBus>();

        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<EventHandlerAttribute>();
            var eventType = attribute!.Type;
            if (string.IsNullOrEmpty(eventType))
                eventType = method.Name;

            if (eventType.StartsWith("On"))
                eventType = eventType[2..];

            if (eventType.EndsWith("Handler"))
                eventType = eventType[..^7];

            if (eventType.EndsWith("Event"))
                eventType = eventType[..^5];

            eventBus.Subscribe(eventType, async evt =>
            {
                using var scope = provider.CreateScope();
                var obj = method.IsStatic ? 
                    null : 
                    ActivatorUtilities.CreateInstance(scope.ServiceProvider, method.DeclaringType!);
                var args = new object[] { evt };
                var result = method.Invoke(obj, args);
                if (result is Task task)
                    await task;
            });
        }
    }
}
