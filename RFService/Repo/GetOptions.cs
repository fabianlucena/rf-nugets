using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RFService.Libs;

namespace RFService.Repo
{
    public class GetOptions
    {
        public int? Offset { get; set; }
        
        public int? Top { get; set; }

        public Dictionary<string, object?> Filters { get; set; } = [];
        
        public Dictionary<string, GetOptions> Include { get; set; } = [];

        public GetOptions() { }

        public GetOptions(GetOptions options)
        {
            Offset = options.Offset;
            Top = options.Top;
            Filters = new Dictionary<string, object?>(options.Filters);
            Include = new Dictionary<string, GetOptions>(options.Include);
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

            return this;
        }
    }
}
