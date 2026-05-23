namespace RFIServices.QueryOptions
{
    public sealed class EntityQueryOptionsClonable : EntityQueryOptions
    {
        public EntityQueryOptionsClonable() { }

        public EntityQueryOptionsClonable(EntityQueryOptionsClonable? options)
            : base(options)
        {
            if (options == null)
                return;
        }

        public override EntityQueryOptionsClonable Clone()
            => new(this);
    }
}
