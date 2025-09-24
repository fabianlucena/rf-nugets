namespace RFService.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SizeAttribute(int size) : Attribute
    {
        public int Size { get; } = size;
    }
}

