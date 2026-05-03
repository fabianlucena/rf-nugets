using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class NominableEntityQueryOptions : CommonEntityQueryOptions
    {
        public string? Name { get; set; }

        public NominableEntityQueryOptions() { }

        public NominableEntityQueryOptions(NominableEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Name = options.Name;
        }

        public override NominableEntityQueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new NominableEntityQueryOptions(this);
        }
    }
}
