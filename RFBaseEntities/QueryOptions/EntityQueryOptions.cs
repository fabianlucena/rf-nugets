using RFBaseEntities.Entities;
using RFBaseEntities.Exceptions;

namespace RFBaseEntities.QueryOptions
{
    public class EntityQueryOptions : BaseQueryOptions
    {
        public long? Id { get; set; }
        public Guid? Uuid { get; set; }

        public EntityQueryOptions() { }

        public EntityQueryOptions(EntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Id = options.Id;
            Uuid = options.Uuid;
        }

        public override EntityQueryOptions Clone()
        {
            if (this.GetType() == typeof(Base))
                throw new CloneMethodMustBeOverridedInDerivatedClassException();

            return new EntityQueryOptions(this);
        }
    }
}
