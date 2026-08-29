using Microsoft.AspNetCore.Http;

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

        public QueryOptions BuildFromRequest(HttpRequest request, QueryOptions options)
        {
            if (request.Query.ContainsKey("skip"))
                options.Skip = int.Parse(request.Query["skip"].ToString());

            if (request.Query.ContainsKey("take"))
                options.Take = int.Parse(request.Query["take"].ToString());

            if (request.Query.ContainsKey("distinct"))
                options.Distinct = bool.Parse(request.Query["distinct"].ToString());

            return options;
        }
    }
}
