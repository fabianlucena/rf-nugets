namespace RFIServices.QueryOptions
{
    public abstract class CommonJoinQueryOptions : BaseQueryOptions
    {
        public bool IncludeDeleted { get; set; } = false;
        public bool IncludeDeletedBy { get; set; } = false;

        public CommonJoinQueryOptions() { }

        public CommonJoinQueryOptions(CommonJoinQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeDeleted = options.IncludeDeleted;
            IncludeDeletedBy = options.IncludeDeletedBy;
        }
    }
}
