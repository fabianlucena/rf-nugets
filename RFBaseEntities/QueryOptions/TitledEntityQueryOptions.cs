namespace RFBaseEntities.QueryOptions
{
    public class TitledEntityQueryOptions : NominableEntityQueryOptions
    {
        public string? Title { get; init; }

        public TitledEntityQueryOptions() { }

        public TitledEntityQueryOptions(TitledEntityQueryOptions options)
            : base(options)
        {
            Title = options.Title;
        }
    }
}
