using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class JoinQueryOptions : BaseQueryOptions
    {
        public JoinQueryOptions() { }

        public JoinQueryOptions(JoinQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;
        }

        public override JoinQueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new JoinQueryOptions(this);
        }
    }
}
