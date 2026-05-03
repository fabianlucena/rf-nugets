using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class CreatableEntityQueryOptions : EntityQueryOptions
    {
        public bool IncludeCreatedBy { get; set; } = false;

        public CreatableEntityQueryOptions() { }

        public CreatableEntityQueryOptions(CreatableEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeCreatedBy = options.IncludeCreatedBy;
        }

        public override CreatableEntityQueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new CreatableEntityQueryOptions(this);
        }
    }
}
