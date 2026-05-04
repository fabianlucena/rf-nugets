namespace RFBaseEntities.QueryOptions
{
    public abstract class TitledEntityQueryOptions : NominableEntityQueryOptions
    {
        public string? Title { get; init; }

        public TitledEntityQueryOptions() { }

        public TitledEntityQueryOptions(TitledEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Title = options.Title;
        }
    }
}
