using RFOperators;
using RFService.Repo;
using System.Data.Common;
using System.Reflection;

namespace RFDapper
{
    public interface IDriver
    {
        bool UseUpdateFromAlias { get; }
        bool UseUpdateSetFrom { get; }

        public DbConnection OpenConnection(string? connectionString = null);

        string GetDefaultSchema();

        string GetSchemaName(string schema);

        string GetCreateSchemaIfNotExistsQuery(string schemaName);

        string SanitizeVarName(string varname);

        string GetParamName(string paramName, List<string> usedNames);

        string GetColumnName(string columnName, QueryOptions? options = null, string? defaultAlias = null);

        string GetTableName(string tableName, string? defaultScheme = null);

        string GetTableAlias(string tableAlias);

        string GetColumnAlias(string columnAlias);

        string GetContraintName(string contraintName);

        string GetClusteredQuery();

        string GetNonClusteredQuery();

        string GetCreateTableIfNotExistsQuery(string tableName, string tableDefinition, string? schemaName = null);

        SqlQuery GetValue(object? data, QueryOptions options, List<string> usedNames, string paramName);

        SqlQuery GetBool(SqlQuery sqlQuery);

        string? GetColumnType(string type, PropertyInfo property);

        string GetSqlSelectedProperty(PropertyInfo property, QueryOptions options, string? defaultAlias = null);

        string? GetSqlColumnDefinition(PropertyInfo property);

        string GetJoinType(JoinType joinType);

        string GetSqlOrderBy(string orderBy, QueryOptions options);

        IEnumerable<string> GetSqlOrderBy(IEnumerable<string> orderBy, QueryOptions options);

        string GetSqlLimit(QueryOptions options);

        string GetSelectLastIdQuery();

        string GetDataLength(string sqlQuery);

        SqlQuery? GetOperation(Operator op, QueryOptions options, List<string> usedNames, string paramName, Func<Operator, QueryOptions, List<string>, string, SqlQuery> getOperation)
            => null;
    }
}
