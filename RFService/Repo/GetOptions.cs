using Microsoft.AspNetCore.Http;
using RFOperators;
using RFService.Libs;
using System.Data;
using System.Globalization;
using static ODataParser;

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

        public Operators Filters { get; set; } = [];

        public List<string> OrderBy { get; set; } = [];

        public bool IncludeDisabled { get; set; } = false;

        public bool IncludeDeleted { get; set; } = false;

        public GetOptions() { }

        public GetOptions(GetOptions? options)
            : base(options)
        {
            if (options != null)
            {
                RepoOptions = options.RepoOptions;
                Offset = options.Offset;
                Top = options.Top;
                Buffered = options.Buffered;
                Separator = options.Separator;
                CommandTimeout = options.CommandTimeout;
                CommandType = options.CommandType;
                Filters = [.. options.Filters];
                OrderBy = options.OrderBy;
                IncludeDisabled = options.IncludeDisabled;
                IncludeDeleted = options.IncludeDeleted;
            }
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
                AddFilterIfNotExists("Uuid", uuid);

            return this;
        }

        public GetOptions AddFromQuery(IQueryCollection query)
        {
            if (query.TryGetBool("IncludeDisabled", out bool includeDisabled))
                IncludeDisabled = includeDisabled;

            if (query.TryGetBool("IncludeDeleted", out bool includeDeleted))
                IncludeDeleted = includeDeleted;

            var filters = query.GetStrings("$filter");
            foreach (var filter in filters)
            {
                if (string.IsNullOrEmpty(filter))
                    continue;

                var tree = ODataParser.ParseOData(filter);
                AddFilter(ODataTreeToFilter(tree));
            }

            return this;
        }

        static Operator ODataTreeToFilter(Nodo nodo)
        {
            switch (nodo)
            {
                case Comparation c:
                    var field = c.Field;
                    field = char.ToUpper(field[0], CultureInfo.InvariantCulture) + field[1..];

                    return c.Operator switch
                    {
                        "eq" => Op.Eq(field, c.Value),
                        "ne" => Op.NE(field, c.Value),
                        "gt" => Op.GT(field, c.Value),
                        "lt" => Op.LT(field, c.Value),
                        "ge" => Op.GE(field, c.Value),
                        "le" => Op.LE(field, c.Value),
                        _ => throw new Exception($"Unknown operator: {c.Operator}"),
                    };

                case Function:
                    //Console.WriteLine($"Función: {f.Name}({f.Field}, '{f.Argument}')");
                    throw new Exception("OData function not yet implemented.");

                case ODataParser.Binary b:
                    return b.Operator switch
                    {
                        "and" => Op.And(ODataTreeToFilter(b.Left), ODataTreeToFilter(b.Right)),
                        "or" => Op.Or(ODataTreeToFilter(b.Left), ODataTreeToFilter(b.Right)),
                        _ => throw new Exception($"Unknown operator: {b.Operator}"),
                    };

                default: throw new Exception("Unknown OData node.");
            };
        }

        public bool HasColumnFilter(string column)
            => Filters.Any(filter => filter.HasColumn(column));

        public GetOptions AddFilterIfNotExists(string column, object? value)
        {
            if (!HasColumnFilter(column))
                AddFilter(column, value);

            return this;
        }

        public GetOptions AddFilter(string column, object? value)
        {
            Filters.Add(column, value);

            return this;
        }

        public GetOptions AddFilter(Operator op)
        {
            Filters.Add(op);

            return this;
        }

        public GetOptions SetFilter<T>(string column, object? value)
            where T : Operator
        {
            if (!Filters.SetForColumn<T>(column, value))
            {
                var newFilter = (T?)Activator.CreateInstance(typeof(T), column, value)
                    ?? throw new Exception("Unknown operator.");

                AddFilter(newFilter);
            }

            return this;
        }

        public GetOptions Include(
            string propertyName,
            string? alias = null,
            JoinType? type = null,
            Operator? on = null,
            Type? entity = null
        )
        {
            Join.Add(new From(propertyName: propertyName, alias: alias, type: type, on: on, entity: entity));
            return this;
        }
    }
}
