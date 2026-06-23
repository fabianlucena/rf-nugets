using Microsoft.Extensions.DependencyInjection;

namespace RFRegisterService.Attributes;

public class RegisterServiceAttributeBase :  Attribute
{
    public ServiceLifetime Lifetime { get; } = ServiceLifetime.Scoped;
    public Type[]? Interfaces { get; } = null;

    public RegisterServiceAttributeBase()
    {
    }

    public RegisterServiceAttributeBase(
        ServiceLifetime lifetime,
        params Type[] interfaces
    )
    {
        Lifetime = lifetime;
        Interfaces = interfaces;
    }

    public RegisterServiceAttributeBase(ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        Lifetime = lifetime;
    }

    public RegisterServiceAttributeBase(params Type[] interfaces)
    {
        Interfaces = interfaces;
    }
}
