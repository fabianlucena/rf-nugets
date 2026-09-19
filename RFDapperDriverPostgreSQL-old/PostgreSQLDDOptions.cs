using Npgsql;
using RFDapper;
using RFService.Repo;
using System.Reflection;

namespace RFDapperDriverPostgreSQL
{
    public class PostgreSQLDDOptions
    {
        public string? ConnectionString { get; set; }

        public NpgsqlDataSource? DataSource { get; set; }

        public Action<NpgsqlDataSourceBuilder>? PrepareDataSourceBuilder { get; set; } = null;

        public Dictionary<string, Func<PropertyInfo, string>> ColumnTypes { get; set; } = [];

        public Func<IDriver, PropertyInfo, QueryOptions, string?, string?>? GetSqlSelectedProperty { get; set; } = null;
    }
}
