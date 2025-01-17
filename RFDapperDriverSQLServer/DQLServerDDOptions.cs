using RFDapper;
using RFService.Repo;
using System.Reflection;

namespace RFDapperDriverSQLServer
{
    public class DQLServerDDOptions
    {
        public Dictionary<string, string> ColumnTypes { get; set; } = [];

        public Func<IDriver, PropertyInfo, GetOptions, string?, string?>? GetSqlSelectedProperty { get; set; } = null;
    }
}
