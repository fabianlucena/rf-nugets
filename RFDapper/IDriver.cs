using RFService.Repo;
using System.Reflection;

namespace RFDapper
{
    public interface IDriver
    {
        string GetDefaultSchema();

        string GetSchemaName(string schema);

        string SanitizeVarname(string varname);

        string GetParamName(string paramName, List<string> usedNames);

        string GetColumnName(string key, GetOptions options, string? defaultAlias = null);

        string GetTableName(string tableName, string? defaultScheme = null);

        string GetTableAlias(string tableAlias);

        string GetColumnAlias(string columnAlias);

        SqlQuery GetValue(object? data, GetOptions options, List<string> usedNames);

        string? GetColumnType(string type, PropertyInfo property);

        string GetSqlSelectedProperty(PropertyInfo property, GetOptions options, string? defaultAlias = null);

        string? GetSqlColumnDefinition(PropertyInfo property);

        string GetSqlOrderBy(string orderBy, GetOptions options);

        IEnumerable<string> GetSqlOrderBy(IEnumerable<string> orderBy, GetOptions options);
    }
}
