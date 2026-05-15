namespace RFBaseEntities.Entities
{
    public abstract class Base
    {
        public Base() { }

        public Base(Base? _) { }

        public abstract Base Clone();
    }
}
