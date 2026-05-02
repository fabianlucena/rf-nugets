namespace RFBaseEntities.QueryOptions
{
    public class NominableEntityQueryOptions : CommonEntityQueryOptions
    {
        public string? Name { get; init; }

        public NominableEntityQueryOptions() { }

        public NominableEntityQueryOptions(NominableEntityQueryOptions options)
            : base(options)
        {
            Name = options.Name;
        }
    }
}
