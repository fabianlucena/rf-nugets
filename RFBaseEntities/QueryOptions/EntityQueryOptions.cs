namespace RFBaseEntities.QueryOptions
{
    public abstract class EntityQueryOptions : BaseQueryOptions
    {
        public long? Id { get; set; }
        public Guid? Uuid { get; set; }

        public bool SkipOrderById { get; set; }

        public EntityQueryOptions() { }

        public EntityQueryOptions(EntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Id = options.Id;
            Uuid = options.Uuid;

            SkipOrderById = options.SkipOrderById;
        }
    }
}
