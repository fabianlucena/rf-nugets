using Microsoft.Extensions.DependencyInjection;

namespace RFServices.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RegisterServiceAttribute :  Attribute
    {
        public ServiceLifetime Lifetime { get; } = ServiceLifetime.Scoped;
        public Type[]? Interfaces { get; } = null;

        public RegisterServiceAttribute()
        {
        }

        public RegisterServiceAttribute(
            ServiceLifetime lifetime,
            params Type[] interfaces
        )
        {
            Lifetime = lifetime;
            Interfaces = interfaces;
        }

        public RegisterServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Lifetime = lifetime;
        }

        public RegisterServiceAttribute(params Type[] interfaces)
        {
            Interfaces = interfaces;
        }
    }
}

