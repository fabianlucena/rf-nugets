using RFDapper;
using RFService.Repo;
using System.Text.RegularExpressions;

namespace RFDapperDriverSQLServer
{
    public partial class SQLServerDD
        : IDriver
    {
        private readonly static Regex SqareBracketColumn= new(@"^\[.*\]$");
        private readonly static Regex SqareBracketTableAndColumn= new(@"^\[.*\]\.\[.*\]$");
        private readonly static Regex SqareBracketTableAndFreeColumn = new(@"^\[.*\]\.[\w][\w\d]*$");
        private readonly static Regex FreeTableAndSqareBracketColumn = new(@"^[\w][\w\d]\.\[.*\]*$");

        public string GetDefaultSchema()
            => "[dbo]";

        public string SanitizeSchema(string schema)
        {
            return schema;
        }

        public string SanitizeVarname(string name)
        {
            return name.Replace('.', '_');
        }

        public string GetFullColumnName(string columnName, GetOptions options, string? defaultAlias)
        {
            columnName = columnName.Trim();
            if (SqareBracketColumn.IsMatch(columnName)
                || SqareBracketTableAndColumn.IsMatch(columnName)
                || SqareBracketTableAndFreeColumn.IsMatch(columnName)
                || FreeTableAndSqareBracketColumn.IsMatch(columnName)
            )
                return columnName;

            if (columnName.Contains('[') || columnName.Contains(']'))
                throw new InvalidColumnNameException(columnName);

            var parts = columnName.Split('.');
            if (parts.Length > 2)
                throw new InvalidColumnNameException(columnName);

            var index = parts.Length - 1;

            columnName = $"[{parts[index]}]";
            index--;

            if (index >= 0)
                columnName = $"[{parts[index]}].{columnName}";
            else if (!string.IsNullOrEmpty(defaultAlias))
                columnName = $"[{defaultAlias}].{columnName}";
            
            return columnName;
        }
    }
}
