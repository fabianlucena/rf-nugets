using Microsoft.AspNetCore.Http;
using RFService.Libs;
using System.Data;

namespace RFService.Repo
{
    public class GetOptions
        : From,
            IDisposable
    {
        public RepoOptions RepoOptions { get; set; } = new RepoOptions();

        public int? Offset { get; set; }

        public int? Top { get; set; }

        public bool Buffered { get; set; } = true;

        public string Separator { get; set; } = "*** S E P A R A T O R ***";

        public int? CommandTimeout { get; set; } = null;

        public CommandType? CommandType { get; set; } = null;

        public Dictionary<string, object?> Filters { get; set; } = [];

        public List<string> OrderBy { get; set; } = [];

        public Dictionary<string, object?> Options { get; set; } = [];

        public GetOptions() { }

        public GetOptions(GetOptions options)
            : base(options)
        {
            RepoOptions = options.RepoOptions;
            Offset = options.Offset;
            Top = options.Top;
            Buffered = options.Buffered;
            Separator = options.Separator;
            CommandTimeout = options.CommandTimeout;
            CommandType = options.CommandType;
            Filters = new Dictionary<string, object?>(options.Filters);
            OrderBy = options.OrderBy;
            Options = new Dictionary<string, object?>(options.Options);
        }

        public GetOptions(RepoOptions repoOptions)
        {
            RepoOptions = repoOptions;
        }

        ~GetOptions()
            => Dispose();

        public void Dispose()
        {
            RepoOptions.Dispose();
            GC.SuppressFinalize(this);
        }

        public static GetOptions CreateFromQuery(IQueryCollection query)
        {
            GetOptions options = new();
            options.AddFromQuery(query);

            return options;
        }

        public static GetOptions CreateFromQuery(HttpContext httpContext)
            => CreateFromQuery(httpContext.Request.Query.GetPascalized());
        
        public GetOptions AddFilterUuid(Guid? uuid)
        {
            if (uuid != null)
                Filters["Uuid"] = uuid;

            return this;
        }

        public GetOptions AddFromQuery(IQueryCollection query)
        {
            if (!query.ContainsKey("IncludeDisabled"))
                Filters["IsEnabled"] = true;

            if (query.TryGetBool("IncludeDeleted", out bool includeDeleted))
                Options["IncludeDeleted"] = includeDeleted;

            return this;
        }

        public GetOptions Include(string propertyName, string? alias = null)
        {
            Join[propertyName] = new From(alias);
            return this;
        }
    }
}
