namespace RFIServices.QueryOptions
{
    public class NominableEntityQueryOptions : CommonEntityQueryOptions
    {
        public string? Name { get; set; }
        public IEnumerable<string>? Names { get; set; }

        public NominableEntityQueryOptions() { }

        public NominableEntityQueryOptions(NominableEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Name = options.Name;
            Names = options.Names;
        }

        public override NominableEntityQueryOptions Clone()
            => new(this);
    }
}
