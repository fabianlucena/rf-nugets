using RFBaseEntities.Exceptions;

namespace RFBaseEntities.Entities
{
    public class CommonJoin : CreatableJoin
    {
        public DateTime? DeletedAt { get; set; } = null;

        public long? DeletedById { get; set; } = 0;

        public User? DeletedBy { get; set; } = null;

        public CommonJoin() { }

        public CommonJoin(CommonJoin entity)
            : base(entity)
        {
            DeletedAt = entity.DeletedAt;
            DeletedById = entity.DeletedById;
            DeletedBy = entity.DeletedBy;
        }

        public override CommonJoin Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new CommonJoin(this);
        }
    }
}