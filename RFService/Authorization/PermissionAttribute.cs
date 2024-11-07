namespace RFService.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class PermissionAttribute(params string[] permissions) : Attribute
    {
        public string[] Permissions { get; set; } = permissions;
    }
}
