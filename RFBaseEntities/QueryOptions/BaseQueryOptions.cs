using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class BaseQueryOptions : QueryOptions
    {
        public BaseQueryOptions() { }
    
        public BaseQueryOptions(BaseQueryOptions? options)
            : base(options)
        {
        }

        public override BaseQueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new BaseQueryOptions(this);
        }
    }
}
