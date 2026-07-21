using Microsoft.Extensions.DependencyInjection;

namespace RFRegisterService.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RegisterServiceAttribute : RegisterServiceAttributeBase
{
}

