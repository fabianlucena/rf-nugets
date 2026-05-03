using RFBaseEntities.Exceptions;

namespace RFBaseEntities.Entities
{
    public class ImmutableEntity : CreatableEntity
    {
        public DateTime? DeletedAt { get; set; } = null;

        public long? DeletedById { get; set; } = 0;

        public User? DeletedBy { get; set; } = null;

        public ImmutableEntity() { }

        public ImmutableEntity(ImmutableEntity entity)
            : base(entity)
        {
            DeletedAt = entity.DeletedAt;
            DeletedById = entity.DeletedById;
            DeletedBy = entity.DeletedBy;
        }

        public override ImmutableEntity Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new ImmutableEntity(this);
        }
    }
}
