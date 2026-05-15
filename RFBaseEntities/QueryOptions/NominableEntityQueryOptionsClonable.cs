namespace RFBaseEntities.QueryOptions
{
    public sealed class NominableEntityQueryOptionsClonable : NominableEntityQueryOptions
    {
        public NominableEntityQueryOptionsClonable() { }

        public NominableEntityQueryOptionsClonable(NominableEntityQueryOptionsClonable? options)
            : base(options)
        {
            if (options == null)
                return;

            Name = options.Name;
        }

        public override NominableEntityQueryOptionsClonable Clone()
            => new(this);
    }
}
