using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class QueryOptions
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 20;

        public QueryOptions() { }

        public QueryOptions(QueryOptions? options)
        {
            if (options == null)
                return;

            Skip = options.Skip;
            Take = options.Take;
        }

        public virtual QueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new QueryOptions(this);
        }
    }
}
