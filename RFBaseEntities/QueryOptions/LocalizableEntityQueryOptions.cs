namespace RFBaseEntities.QueryOptions
{
    public abstract class LocalizableEntityQueryOptions : TitledEntityQueryOptions
    {
        public bool Translate { get; set; }

        public LocalizableEntityQueryOptions() { }

        public LocalizableEntityQueryOptions(LocalizableEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Translate = options.Translate;
        }
    }
}
