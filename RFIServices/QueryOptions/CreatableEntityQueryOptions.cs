namespace RFIServices.QueryOptions
{
    public abstract class CreatableJoinQueryOptions : JoinQueryOptions
    {
        public bool IncludeCreatedBy { get; set; } = false;

        public CreatableJoinQueryOptions() { }

        public CreatableJoinQueryOptions(CreatableJoinQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeCreatedBy = options.IncludeCreatedBy;
        }
    }
}