using RFBaseEntities.Exceptions;

namespace RFBaseEntities.Entities
{
    public class Base
    {
        public Base() { }

        public Base(Base _) { }

        public virtual Base Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new Base(this);
        }
    }
}
