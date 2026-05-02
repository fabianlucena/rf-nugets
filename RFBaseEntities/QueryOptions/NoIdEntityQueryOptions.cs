namespace RFBaseEntities.QueryOptions
{
    public class NoIdEntityQueryOptions : BaseQueryOptions
    {
        public bool IncludeCreatedBy { get; set; } = false;
        public bool IncludeUpdatedBy { get; set; } = false;
        public bool IncludeDeleted { get; set; } = false;
        public bool IncludeDeletedBy { get; set; } = false;

        public NoIdEntityQueryOptions() { }

        public NoIdEntityQueryOptions(NoIdEntityQueryOptions options)
            : base(options)
        {
            IncludeCreatedBy = options.IncludeCreatedBy;
            IncludeUpdatedBy = options.IncludeUpdatedBy;
            IncludeDeleted = options.IncludeDeleted;
            IncludeDeletedBy = options.IncludeDeletedBy;
        }
    }
}
