namespace RFBaseEntities.QueryOptions
{
    public class AuditableEntityQueryOptions : CreatableEntityQueryOptions
    {
        public bool IncludeUpdatedBy { get; set; }

        public AuditableEntityQueryOptions() { }

        public AuditableEntityQueryOptions(AuditableEntityQueryOptions options)
            : base(options)
        {
            IncludeUpdatedBy = options.IncludeUpdatedBy;
        }
    }
}
