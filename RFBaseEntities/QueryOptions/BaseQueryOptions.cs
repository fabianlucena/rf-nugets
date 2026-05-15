namespace RFBaseEntities.QueryOptions
{
    public abstract class BaseQueryOptions : QueryOptions
    {
        public BaseQueryOptions() { }
    
        public BaseQueryOptions(BaseQueryOptions? options)
            : base(options)
        {
        }
    }
}
