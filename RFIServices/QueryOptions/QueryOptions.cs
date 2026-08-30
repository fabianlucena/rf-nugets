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

        public virtual QueryOptions BuildFromRequest(HttpRequest request)
        {
            if (request.Query.ContainsKey("skip"))
                Skip = int.Parse(request.Query["skip"].ToString());

            if (request.Query.ContainsKey("take"))
                Take = int.Parse(request.Query["take"].ToString());

            if (request.Query.ContainsKey("distinct"))
                Distinct = bool.Parse(request.Query["distinct"].ToString());

            return this;
        }
    }
}
