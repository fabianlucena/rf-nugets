namespace RFRegisterService.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RegisterServiceAttribute : RegisterServiceAttributeBase
{
    public RegisterServiceAttribute()
    {}

    public RegisterServiceAttribute(params Type[] interfaces)
        : base(interfaces) { }
}
