namespace RFIServices.QueryOptions
{
    public abstract class QueryOptions
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 20;
        public bool Distinct { get; set; }

        public QueryOptions() { }

        public QueryOptions(QueryOptions? options)
        {
            if (options == null)
                return;

            Skip = options.Skip;
            Take = options.Take;
            Distinct = options.Distinct;
        }

        public abstract QueryOptions Clone();
    }
}
