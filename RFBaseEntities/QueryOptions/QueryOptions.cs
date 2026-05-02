namespace RFBaseEntities.QueryOptions
{
    public class QueryOptions
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 20;

        public QueryOptions() { }

        public QueryOptions(QueryOptions options)
        {
            Skip = options.Skip;
            Take = options.Take;
        }
    }
}
