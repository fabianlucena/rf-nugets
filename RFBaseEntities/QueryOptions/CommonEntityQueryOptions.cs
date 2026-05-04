namespace RFBaseEntities.QueryOptions
{
    public abstract class CommonEntityQueryOptions : AuditableEntityQueryOptions
    {
        public bool IncludeDeleted { get; set; } = false;
        public bool IncludeDeletedBy { get; set; } = false;

        public CommonEntityQueryOptions() { }

        public CommonEntityQueryOptions(CommonEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeDeleted = options.IncludeDeleted;
            IncludeDeletedBy = options.IncludeDeletedBy;
        }
    }
}
