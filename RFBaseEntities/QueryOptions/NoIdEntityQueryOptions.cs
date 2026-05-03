using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class NoIdEntityQueryOptions : BaseQueryOptions
    {
        public bool IncludeCreatedBy { get; set; } = false;
        public bool IncludeUpdatedBy { get; set; } = false;
        public bool IncludeDeleted { get; set; } = false;
        public bool IncludeDeletedBy { get; set; } = false;

        public NoIdEntityQueryOptions() { }

        public NoIdEntityQueryOptions(NoIdEntityQueryOptions options)
            : base(options)
        {
            IncludeCreatedBy = options.IncludeCreatedBy;
            IncludeUpdatedBy = options.IncludeUpdatedBy;
            IncludeDeleted = options.IncludeDeleted;
            IncludeDeletedBy = options.IncludeDeletedBy;
        }

        public override NoIdEntityQueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new NoIdEntityQueryOptions(this);
        }
    }
}
