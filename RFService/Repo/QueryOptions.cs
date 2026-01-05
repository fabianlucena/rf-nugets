using Microsoft.AspNetCore.Http;
using RFOperators;
using RFService.Exceptions;
using RFService.Libs;
using System.Data;
using System.Globalization;
using static ODataParser;

namespace RFService.Repo
{
    public class QueryOptions
        : From,
            IDisposable
    {
        public RepoOptions RepoOptions { get; set; } = new RepoOptions();

        public IEnumerable<Operator>? Select { get; set; } = null;

        public int? Offset { get; set; }

        public int? Top { get; set; }

        public bool Buffered { get; set; } = true;

        public string Separator { get; set; } = "*** S E P A R A T O R ***";

        public int? CommandTimeout { get; set; } = null;

        public CommandType? CommandType { get; set; } = null;

        public Operators Filters { get; set; } = [];

        public List<string> OrderBy { get; set; } = [];

        public Dictionary<string, bool> Switches { get; set; } = [];

        public QueryOptions() { }

        public QueryOptions(QueryOptions? options)
            : base(options)
        {
            if (options != null)
            {
                RepoOptions = options.RepoOptions;
                Select = options.Select;
                Offset = options.Offset;
                Top = options.Top;
                Buffered = options.Buffered;
                Separator = options.Separator;
                CommandTimeout = options.CommandTimeout;
                CommandType = options.CommandType;
                Filters = [.. options.Filters];
                OrderBy = options.OrderBy;
                Switches = new Dictionary<string, bool>(options.Switches);
            }
        }

        public QueryOptions(RepoOptions repoOptions)
        {
            RepoOptions = repoOptions;
        }

        ~QueryOptions()
            => Dispose();

        public void Dispose()
        {
            RepoOptions.Dispose();
            GC.SuppressFinalize(this);
        }

        public static QueryOptions CreateFromQuery(IQueryCollection query)
        {
            QueryOptions options = new();
            options.AddFromQuery(query);

            return options;
        }

        public static QueryOptions CreateFromQuery(HttpContext httpContext)
            => CreateFromQuery(httpContext.Request.Query.GetPascalized());

        public bool GetSwitch(string key, bool defaultValue = false)
            => Switches.TryGetValue(key, out bool value) && value || defaultValue;

        public QueryOptions AddFilterUuid(Guid? uuid)
        {
            if (uuid != null)
                AddFilterIfNotExists("Uuid", uuid);

            return this;
        }

        public QueryOptions AddFromQuery(IQueryCollection query)
        {
            if (query.TryGetBool("IncludeDisabled", out bool includeDisabled))
                Switches["IncludeDisabled"] = includeDisabled;

            if (query.TryGetBool("IncludeDeleted", out bool includeDeleted))
                Switches["IncludeDeleted"] = includeDeleted;

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

        public Operator GetColumnFilter(string column)
            => Filters.Find(filter => filter.HasColumn(column))
            ?? throw new FilterForColumnDoesNosExistException(column);

        public QueryOptions SetColumnFilter(string column, object? value)
        {
            if (HasColumnFilter(column))
                GetColumnFilter(column).SetForColumn(column, value);
            else
                AddFilter(column, value);

            return this;
        }

        public QueryOptions AddFilterIfNotExists(string column, object? value)
        {
            if (!HasColumnFilter(column))
                AddFilter(column, value);

            return this;
        }

        public QueryOptions AddFilter(string column, object? value)
        {
            Filters.Add(column, value);

            return this;
        }

        public QueryOptions AddFilter(Operator op)
        {
            Filters.Add(op);

            return this;
        }

        public QueryOptions SetFilter<T>(string column, object? value)
            where T : Operator
        {
            if (!Filters.SetForColumn(column, value))
            {
                var newFilter = (T?)Activator.CreateInstance(typeof(T), column, value)
                    ?? throw new Exception("Unknown operator.");

                AddFilter(newFilter);
            }

            return this;
        }

        public QueryOptions Include(
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

        public QueryOptions IncludeIfNotExists(
            string propertyName,
            string? alias = null,
            JoinType? type = null,
            Operator? on = null,
            Type? entity = null
        )
        {
            if (!string.IsNullOrEmpty(alias)
                && Join.Exists(j => j.Alias == alias)
            )
            {
                return this;
            }

            Join.Add(new From(propertyName: propertyName, alias: alias, type: type, on: on, entity: entity));
            return this;
        }
    }
}
