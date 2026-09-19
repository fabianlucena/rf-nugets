using RFDapper;
using RFService.Repo;
using System.Reflection;

namespace RFDapperDriverSQLServer
{
    public class SQLServerDDOptions
    {
        public string? ConnectionString { get; set; }

        public Dictionary<string, string> ColumnTypes { get; set; } = [];

        public Func<IDriver, PropertyInfo, QueryOptions, string?, string?>? GetSqlSelectedProperty { get; set; } = null;
    }
}
