using RFBaseEntities.Exceptions;

namespace RFBaseEntities.Entities
{
    public class CreatableJoin : Join
    {
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public long CreatedById { get; set; } = 0;
        public User? CreatedBy { get; set; } = null;

        public CreatableJoin() { }

        public CreatableJoin(CreatableJoin entity)
            : base(entity)
        {
            CreatedAt = entity.CreatedAt;
            CreatedById = entity.CreatedById;
            CreatedBy = entity.CreatedBy;
        }

        public override CreatableJoin Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new CreatableJoin(this);
        }
    }
}
