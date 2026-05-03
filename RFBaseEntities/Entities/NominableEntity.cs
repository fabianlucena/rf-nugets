using RFBaseEntities.Exceptions;

namespace RFBaseEntities.Entities
{
    public class NominableEntity : CommonEntity
    {
        public string Name { get; set; } = string.Empty;

        public NominableEntity() { }

        public NominableEntity(NominableEntity entity)
            : base(entity)
        {
            Name = entity.Name;
        }

        public override NominableEntity Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new NominableEntity(this);
        }
    }
}
