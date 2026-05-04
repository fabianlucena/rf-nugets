namespace RFBaseEntities.QueryOptions
{
    public abstract class NominableEntityQueryOptions : CommonEntityQueryOptions
    {
        public string? Name { get; set; }

        public NominableEntityQueryOptions() { }

        public NominableEntityQueryOptions(NominableEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Name = options.Name;
        }
    }
}
