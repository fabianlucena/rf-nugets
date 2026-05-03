using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class CreatableJoinQueryOptions : JoinQueryOptions
    {
        public bool IncludeCreatedBy { get; set; } = false;

        public CreatableJoinQueryOptions() { }

        public CreatableJoinQueryOptions(CreatableJoinQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeCreatedBy = options.IncludeCreatedBy;
        }

        public override CreatableJoinQueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new CreatableJoinQueryOptions(this);
        }
    }
}