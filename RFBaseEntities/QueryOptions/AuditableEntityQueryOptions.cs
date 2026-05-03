using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class AuditableEntityQueryOptions : CreatableEntityQueryOptions
    {
        public bool IncludeUpdatedBy { get; set; }

        public AuditableEntityQueryOptions() { }

        public AuditableEntityQueryOptions(AuditableEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeUpdatedBy = options.IncludeUpdatedBy;
        }

        public override AuditableEntityQueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new AuditableEntityQueryOptions(this);
        }
    }
}
