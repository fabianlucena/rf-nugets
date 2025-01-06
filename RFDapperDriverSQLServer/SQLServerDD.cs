using Microsoft.IdentityModel.Tokens;
using RFDapper;
using RFService.Repo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.RegularExpressions;
using static Azure.Core.HttpHeader;

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

        public string GetParamName(string paramName, List<string> usedNames)
        {
            paramName = paramName.Trim();
            if (string.IsNullOrEmpty(paramName))
                paramName = "param";

            if (usedNames.Contains(paramName))
            {
                var root = paramName;
                var counter = 0;
                do
                {
                    counter++;
                    paramName = root + "_" + counter;
                } while (usedNames.Contains(paramName));
            }

            usedNames.Add(paramName);
            return $"@{paramName}";
        }

        public string GetColumnName(string columnName, GetOptions options, string? defaultAlias = null)
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
            else if (!string.IsNullOrEmpty(options.Alias))
                columnName = $"[{options.Alias}].{columnName}";

            return columnName;
        }

        public SqlQuery GetValue(
            object? data,
            GetOptions options,
            List<string> usedNames
        )
        {
            var name = GetParamName("", usedNames);

            return new()
            {
                Sql = name,
                Data = new Data { Values = { { name, data } } },
            };
        }

        public string? GetColumnType(string type, PropertyInfo property)
        {
            switch (type)
            {
                case "Int32": return "INT";
                case "Int64": return "BIGINT";
                case "Guid": return "UNIQUEIDENTIFIER";
                case "DateTime": return "DATETIME";
                case "Boolean": return "BIT";
                case "SqlGeography": return "GEOGRAPHY";
                case "String":
                    var length = property.GetCustomAttribute<MaxLengthAttribute>()?.Length
                        ?? property.GetCustomAttribute<LengthAttribute>()?.MaximumLength;

                    if (length.HasValue)
                        return $"NVARCHAR({length.Value})";

                    return "NVARCHAR(MAX)";
            }

            if (type.StartsWith("DECIMAL", StringComparison.CurrentCultureIgnoreCase))
                return type;

            if (!property.PropertyType.IsClass)
                throw new UnknownColumnTypeException(type);

            return null;
        }

        public string? GetSQLColumnDefinition(PropertyInfo property)
        {
            var propertyType = property.PropertyType;
            var columnAttributes = propertyType.GetCustomAttribute<ColumnAttribute>();
            bool? nullable = null;
            string propertyTypeName;
            if (columnAttributes?.TypeName != null)
            {
                propertyTypeName = columnAttributes.TypeName;
            }
            else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                nullable = true;
                propertyTypeName = Nullable.GetUnderlyingType(propertyType)?.Name ?? "";
            }
            else
            {
                propertyTypeName = propertyType.Name;
            }

            var columnType = GetColumnType(propertyTypeName, property);
            if (columnType == null)
                return null;

            var columnDefinition = $"[{property.Name}] {columnType}";

            if (property.CustomAttributes.Any(a => a.AttributeType.Name == "RequiredAttribute"))
                columnDefinition += " NOT NULL";
            else if (Nullable.GetUnderlyingType(propertyType) != null)
                columnDefinition += " NULL";
            else if (nullable != null)
            {
                columnDefinition += (bool)nullable ?
                    " NULL" :
                    " NOT NULL";
            }

            var databaseGeneratedAttribute = property.GetCustomAttribute<DatabaseGeneratedAttribute>();
            if (databaseGeneratedAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                columnDefinition += " IDENTITY(1,1)";

            return columnDefinition;
        }
    }
}
