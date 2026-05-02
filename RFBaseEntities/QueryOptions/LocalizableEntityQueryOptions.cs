namespace RFBaseEntities.QueryOptions
{
    public class LocalizableEntityQueryOptions : TitledEntityQueryOptions
    {
        public bool Translate { get; set; }

        public LocalizableEntityQueryOptions() { }

        public LocalizableEntityQueryOptions(LocalizableEntityQueryOptions options)
            : base(options)
        {
            Translate = options.Translate;
        }
    }
}
