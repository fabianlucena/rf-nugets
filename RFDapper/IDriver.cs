using RFService.Repo;
using System.Reflection;

namespace RFDapper
{
    public interface IDriver
    {
        string GetDefaultSchema();

        string SanitizeSchema(string schema);
        
        string SanitizeVarname(string varname);

        string GetFullColumnName(string key, GetOptions options, string? defaultAlias);

        string? GetColumnType(string type, PropertyInfo property);

        string? GetSQLColumnDefinition(PropertyInfo property);
    }
}
