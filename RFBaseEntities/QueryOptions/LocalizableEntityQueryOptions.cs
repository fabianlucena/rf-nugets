using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class LocalizableEntityQueryOptions : TitledEntityQueryOptions
    {
        public bool Translate { get; set; }

        public LocalizableEntityQueryOptions() { }

        public LocalizableEntityQueryOptions(LocalizableEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Translate = options.Translate;
        }

        public override LocalizableEntityQueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new LocalizableEntityQueryOptions(this);
        }
    }
}
