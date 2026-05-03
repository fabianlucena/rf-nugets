using RFBaseEntities.Exceptions;

namespace RFBaseEntities.Entities
{
    public class Join : Base
    {
        public Join() { }

        public Join(Join entity)
            : base(entity)
        {
        }

        public override Join Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new Join(this);
        }
    }
}
