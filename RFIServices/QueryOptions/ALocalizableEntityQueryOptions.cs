namespace RFIServices.QueryOptions
{
    public abstract class ALocalizableEntityQueryOptions : LocalizableEntityQueryOptions
    {
        public ALocalizableEntityQueryOptions() { }

        public ALocalizableEntityQueryOptions(ALocalizableEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;
        }
    }
}
