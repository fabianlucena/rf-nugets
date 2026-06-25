namespace RFIServices.QueryOptions
{
    public abstract class EntityQueryOptions : BaseQueryOptions
    {
        public long? Id { get; set; }
        public IEnumerable<long>? Ids { get; set; }
        public Guid? Uuid { get; set; }

        public bool SkipOrderById { get; set; }

        public EntityQueryOptions() { }

        public EntityQueryOptions(EntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Id = options.Id;
            Ids = options.Ids;
            Uuid = options.Uuid;

            SkipOrderById = options.SkipOrderById;
        }
    }
}
