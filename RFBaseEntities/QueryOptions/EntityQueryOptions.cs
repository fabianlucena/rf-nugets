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

        public override QueryOptions Clone()
            => new EntityQueryOptions(this);
    }
}
