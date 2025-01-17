namespace RFService.Libs
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class IndexAttribute(params string[] propertyNames) : Attribute
    {
        public IReadOnlyList<string> PropertyNames { get; } = propertyNames;

        public bool IsUniqueHasValue { get; private set; } = false;

        private bool isUnique;
        public bool IsUnique
        {
            get
            {
                return isUnique;
            }

            set
            {
                isUnique = value;
                IsUniqueHasValue = true;
            }
        }

        public string? Name { get; }
    }
}

