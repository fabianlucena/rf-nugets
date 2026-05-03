using RFBaseEntities.Exceptions;

namespace RFBaseEntities.Entities
{
    public class TitledEntity : NominableEntity
    {
        public string Title { get; set; } = string.Empty;

        public TitledEntity() { }

        public TitledEntity(TitledEntity entity)
            : base(entity)
        {
            Title = entity.Title;
        }

        public override TitledEntity Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new TitledEntity(this);
        }
    }
}
