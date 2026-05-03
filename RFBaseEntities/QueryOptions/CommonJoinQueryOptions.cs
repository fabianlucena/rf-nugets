using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class CommonJoinQueryOptions : BaseQueryOptions
    {
        public bool IncludeDeleted { get; set; } = false;
        public bool IncludeDeletedBy { get; set; } = false;

        public CommonJoinQueryOptions() { }

        public CommonJoinQueryOptions(CommonJoinQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeDeleted = options.IncludeDeleted;
            IncludeDeletedBy = options.IncludeDeletedBy;
        }

        public override CommonJoinQueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new CommonJoinQueryOptions(this);
        }
    }
}
