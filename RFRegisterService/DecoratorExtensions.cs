using Microsoft.Extensions.DependencyInjection;

namespace RFRegisterService;

public static class DecoratorExtensions
{
    public static IServiceCollection Decorate(
        this IServiceCollection services,
        Type serviceType,
        Type decoratorType,
        ServiceLifetime lifetime)
    {
        var originalDescriptor = services.First(s => s.ServiceType == serviceType);
        services.Remove(originalDescriptor);

        services.Add(new ServiceDescriptor(
            serviceType,
            provider =>
            {
                var original = provider.CreateInstance(originalDescriptor);
                return ActivatorUtilities.CreateInstance(provider, decoratorType, original);
            },
            lifetime));

        return services;
    }

    private static object CreateInstance(this IServiceProvider provider, ServiceDescriptor descriptor)
    {
        if (descriptor.ImplementationInstance != null)
            return descriptor.ImplementationInstance;

        if (descriptor.ImplementationFactory != null)
            return descriptor.ImplementationFactory(provider);

        if (descriptor.ImplementationType == null)
            throw new InvalidOperationException("Cannot create instance of service descriptor without implementation type.");

        return ActivatorUtilities.CreateInstance(provider, descriptor.ImplementationType);
    }
}
