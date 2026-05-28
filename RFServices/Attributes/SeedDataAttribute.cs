namespace RFServices.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SeedDataAttribute(bool isSystemData = false) : Attribute
    {
        public bool IsSystemData { get; } = isSystemData;
    }
}

