using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class TitledEntityQueryOptions : NominableEntityQueryOptions
    {
        public string? Title { get; init; }

        public TitledEntityQueryOptions() { }

        public TitledEntityQueryOptions(TitledEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Title = options.Title;
        }

        public override TitledEntityQueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new TitledEntityQueryOptions(this);
        }
    }
}
