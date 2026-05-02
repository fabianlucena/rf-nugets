namespace RFBaseEntities.QueryOptions
{
    public class ImmutableEntityQueryOptions : CreatableEntityQueryOptions
    {
        public bool IncludeDeleted { get; set; } = false;
        public bool IncludeDeletedBy { get; set; } = false;

        public ImmutableEntityQueryOptions() { }

        public ImmutableEntityQueryOptions(ImmutableEntityQueryOptions? options)
            : base(options)
        {
            if (options is null)
                return;

            IncludeDeleted = options.IncludeDeleted;
            IncludeDeletedBy = options.IncludeDeletedBy;
        }
    }
}
