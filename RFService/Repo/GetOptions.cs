using Microsoft.AspNetCore.Http;
using RFService.Libs;
using System.Data;

namespace RFService.Repo
{
    public class GetOptions
        : From
    {
        public int? Offset { get; set; }
        
        public int? Top { get; set; }

        public IDbTransaction? Transaction { get; set; }

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
            Offset = options.Offset;
            Top = options.Top;
            Filters = new Dictionary<string, object?>(options.Filters);
        }

        public static GetOptions CreateFromQuery(IDictionary<string, string> query)
        {
            GetOptions options = new();
            options.AddFromQuery(query);

            return options;
        }

        public static GetOptions CreateFromQueryHttpContext(HttpContext httpContext)
        {
            var query = DataValue.GetPascalizeQueryDictionaryFromHttpContext(httpContext);
            return CreateFromQuery(query);
        }

        public GetOptions AddFilterUuid(Guid? uuid)
        {
            if (uuid != null)
                Filters["Uuid"] = uuid;

            return this;
        }

        public GetOptions AddFromQuery(IDictionary<string, string> query)
        {
            if (!query?.ContainsKey("IncludeDisabled") ?? true)
                Filters["IsEnabled"] = true;

            if (query?.TryGetValue("IncludeDeleted", out string? includeDeletedText) ?? false)
            {
                if (bool.TryParse(includeDeletedText, out bool includeDeleted))
                   Options["IncludeDeleted"] = includeDeleted;
            }

            return this;
        }

        public GetOptions Include(string propertyName, string? alias = null)
        {
            Join[propertyName] = new From(alias);
            return this;
        }
    }
}
