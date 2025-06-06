using RFDapper;
using RFService.Repo;
using System.Reflection;

namespace RFDapperDriverMySQL
{
    public class MySQLDDOptions
    {
        public string? ConnectionString { get; set; }

        public string SchemeSeparator { get; set; } = "__";

        public Dictionary<string, string> ColumnTypes { get; set; } = [];

        public Func<IDriver, PropertyInfo, GetOptions, string?, string?>? GetSqlSelectedProperty { get; set; } = null;
    }
}
