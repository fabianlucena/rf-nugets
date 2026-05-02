namespace RFBaseEntities.QueryOptions
{
    public class CommonEntityQueryOptions : AuditableEntityQueryOptions
    {
        public bool IncludeDeleted { get; set; } = false;
        public bool IncludeDeletedBy { get; set; } = false;

        public CommonEntityQueryOptions() { }

        public CommonEntityQueryOptions(CommonEntityQueryOptions options)
            : base(options)
        {
            IncludeDeleted = options.IncludeDeleted;
            IncludeDeletedBy = options.IncludeDeletedBy;
        }
    }
}
