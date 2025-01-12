using RFService.Repo;
using System.Reflection;

namespace RFDapper
{
    public interface IDriver
    {
        string GetDefaultSchema();

        string SanitizeSchema(string schema);

        string SanitizeVarname(string varname);

        string GetParamName(string paramName, List<string> usedNames);

        string GetColumnName(string key, GetOptions options, string? defaultAlias = null);

        SqlQuery GetValue(object? data, GetOptions options, List<string> usedNames);

        string? GetColumnType(string type, PropertyInfo property);

        string? GetSQLColumnDefinition(PropertyInfo property);

        string GetSQLOrderBy(string orderBy, GetOptions options);

        IEnumerable<string> GetSQLOrderBy(IEnumerable<string> orderBy, GetOptions options);
    }
}
