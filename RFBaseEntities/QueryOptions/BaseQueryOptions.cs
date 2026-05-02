namespace RFBaseEntities.QueryOptions
{
    public class BaseQueryOptions : QueryOptions
    {
        public BaseQueryOptions() { }
    
        public BaseQueryOptions(BaseQueryOptions? options)
            : base(options)
        {
        }

        public override QueryOptions Clone()
            => new BaseQueryOptions(this);
    }
}
