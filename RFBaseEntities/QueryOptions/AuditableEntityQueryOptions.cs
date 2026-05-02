namespace RFBaseEntities.QueryOptions
{
    public class AuditableEntityQueryOptions : CreatableEntityQueryOptions
    {
        public bool IncludeUpdatedBy { get; set; }

        public AuditableEntityQueryOptions() { }

        public AuditableEntityQueryOptions(AuditableEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeUpdatedBy = options.IncludeUpdatedBy;
        }
    }
}
