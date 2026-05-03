using RFBaseEntities.Exceptions;

namespace RFBaseEntities.Entities
{
    public class LocalizableEntity : TitledEntity
    {
        public bool IsTranslatable { get; set; }

        public LocalizableEntity() { }

        public LocalizableEntity(LocalizableEntity entity)
            : base(entity)
        {
            IsTranslatable = entity.IsTranslatable;
        }

        public override LocalizableEntity Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new LocalizableEntity(this);
        }
    }
}
