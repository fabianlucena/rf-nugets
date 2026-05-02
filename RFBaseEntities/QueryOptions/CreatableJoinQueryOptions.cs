namespace RFBaseEntities.QueryOptions
{
    public class CreatableEntityQueryOptions : EntityQueryOptions
    {
        public bool IncludeCreatedBy { get; set; } = false;

        public CreatableEntityQueryOptions() { }

        public CreatableEntityQueryOptions(CreatableEntityQueryOptions options)
            : base(options)
        {
            IncludeCreatedBy = options.IncludeCreatedBy;
        }
    }
}
