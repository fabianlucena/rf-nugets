using RFService.Repo;
using System.Data.Common;
using System.Reflection;

namespace RFDapper
{
    public interface IDriver
    {
        bool UseUpdateFrom { get; }

        public DbConnection OpenConnection(string? connectionString = null);

        string GetDefaultSchema();

        string GetSchemaName(string schema);

        string SanitizeVarName(string varname);

        string GetParamName(string paramName, List<string> usedNames);

        string GetColumnName(string columnName, GetOptions? options = null, string? defaultAlias = null);

        string GetTableName(string tableName, string? defaultScheme = null);

        string GetTableAlias(string tableAlias);

        string GetColumnAlias(string columnAlias);

        SqlQuery GetValue(object? data, GetOptions options, List<string> usedNames, string paramName);

        SqlQuery GetBool(SqlQuery sqlQuery);

        string? GetColumnType(string type, PropertyInfo property);

        string GetSqlSelectedProperty(PropertyInfo property, GetOptions options, string? defaultAlias = null);

        string? GetSqlColumnDefinition(PropertyInfo property);

        string GetJoinType(JoinType joinType);

        string GetSqlOrderBy(string orderBy, GetOptions options);

        IEnumerable<string> GetSqlOrderBy(IEnumerable<string> orderBy, GetOptions options);

        string GetSqlLimit(GetOptions options);

        string GetSelectLastIdQuery();
    }
}
