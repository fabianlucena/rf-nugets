using RFBaseEntities.Exceptions;

namespace RFBaseEntities.Entities
{
    public class TranslatableEntity : CommonEntity
    {
        public bool IsTranslatable { get; set; } = false;
        
        public TranslatableEntity() { }

        public TranslatableEntity(TranslatableEntity entity)
            : base(entity)
        {
            IsTranslatable = entity.IsTranslatable;
        }

        public override TranslatableEntity Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new TranslatableEntity(this);
        }
    }
}
