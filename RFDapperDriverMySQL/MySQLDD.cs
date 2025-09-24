using MySqlConnector;
using RFDapper;
using RFDapperDriverMySQL.Exceptions;
using RFService.Libs;
using RFService.Repo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RFDapperDriverMySQL
{
    internal partial class MySQLDD(MySQLDDOptions driverOptions)
        : IDriver
    {
        [GeneratedRegex(@"^`.*`$")]
        private static partial Regex QuotedSingleConstructor();
        private readonly static Regex QuotedSingle = QuotedSingleConstructor();

        [GeneratedRegex(@"^`.*`\.`.*`$")]
        private static partial Regex QuotedDoubleConstructor();
        private readonly static Regex QuotedDouble = QuotedDoubleConstructor();

        [GeneratedRegex(@"^`.*`\.[\w][\w\d]*$")]
        private static partial Regex QuotedAndFreeConstructor();
        private readonly static Regex QuotedAndFree = QuotedAndFreeConstructor();

        [GeneratedRegex(@"^[\w][\w\d]\.`.*`*$")]
        private static partial Regex FreeAndQuotedConstructor();
        private readonly static Regex FreeAndQuoted = FreeAndQuotedConstructor();

        public (DbConnection, Action) OpenConnection()
        {
            if (string.IsNullOrEmpty(driverOptions.ConnectionString))
                throw new NoConnectionStringProvidedException();

            var connection = new MySqlConnection(driverOptions.ConnectionString);
            connection.Open();

            return (
                connection,
                () => connection.Dispose()
            );
        }

        public string GetDefaultSchema()
            => "";

        public string GetSchemaName(string schema)
        {
            return "";
        }

        public string GetCreateSchemaIfNotExistsQuery(string schemaName)
            => "";

        public string SanitizeVarName(string name)
            => name.Replace('.', '_');

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

        public string GetTableName(string tableName, string? defaultScheme = null)
        {
            tableName = tableName.Trim();
            if (QuotedSingle.IsMatch(tableName))
                return tableName;

            if (tableName.Contains('`'))
                throw new InvalidTableNameException(tableName);

            var parts = tableName.Split('.');
            if (parts.Length > 2)
                throw new InvalidTableNameException(tableName);

            var index = parts.Length - 1;
            index--;
            if (index >= 0)
                tableName = $"`{parts[index]}{driverOptions.SchemeSeparator}{tableName}`";
            else if (!string.IsNullOrEmpty(defaultScheme))
                tableName = $"`{defaultScheme}{driverOptions.SchemeSeparator}{tableName}`";

            return tableName;
        }

        public string GetTableAlias(string tableAlias)
        {
            tableAlias = tableAlias.Trim();
            if (QuotedSingle.IsMatch(tableAlias))
                return tableAlias;

            if (tableAlias.Contains('`'))
                throw new InvalidTableAliasException(tableAlias);

            var parts = tableAlias.Split('.');
            if (parts.Length > 1)
                throw new InvalidTableAliasException(tableAlias);

            var index = parts.Length - 1;

            tableAlias = $"`{parts[index]}`";

            return tableAlias;
        }

        public string GetColumnAlias(string columnAlias)
        {
            columnAlias = columnAlias.Trim();
            if (QuotedSingle.IsMatch(columnAlias))
                return columnAlias;

            if (columnAlias.Contains('`') || columnAlias.Contains('`'))
                throw new InvalidColumnAliasException(columnAlias);

            var parts = columnAlias.Split('.');
            if (parts.Length > 1)
                throw new InvalidColumnAliasException(columnAlias);

            var index = parts.Length - 1;

            columnAlias = $"`{parts[index]}`";

            return columnAlias;
        }

        public string GetContraintName(string contraintName)
            => $"`{contraintName}`";

        public string GetClusteredQuery()
            => "";

        public string GetNonClusteredQuery()
            => "";

        public string GetCreateTableIfNotExistsQuery(string tableName, string columnsQuery, string? schemeName)
            => $"CREATE TABLE IF NOT EXISTS {GetTableName(tableName, schemeName)} (\r\n\t{columnsQuery}\r\n)";

        public string GetColumnName(string columnName, QueryOptions? options = null, string? defaultAlias = null)
        {
            columnName = columnName.Trim();
            if (QuotedSingle.IsMatch(columnName)
                || QuotedDouble.IsMatch(columnName)
                || QuotedAndFree.IsMatch(columnName)
                || FreeAndQuoted.IsMatch(columnName)
            )
                return columnName;

            if (columnName.Contains('`'))
                throw new InvalidColumnNameException(columnName);

            var parts = columnName.Split('.');
            if (parts.Length > 2)
                throw new InvalidColumnNameException(columnName);

            var index = parts.Length - 1;

            columnName = $"`{parts[index]}`";
            index--;

            if (index >= 0)
                columnName = $"`{parts[index]}`.{columnName}";
            else if (!string.IsNullOrEmpty(defaultAlias))
                columnName = $"`{defaultAlias}`.{columnName}";
            else if (!string.IsNullOrEmpty(options?.Alias))
                columnName = $"`{options.Alias}`.{columnName}";

            return columnName;
        }

        public SqlQuery GetValue(
            object? data,
            QueryOptions options,
            List<string> usedNames,
            string paramName
        )
        {
            var name = GetParamName(paramName, usedNames);

            return new()
            {
                Sql = name,
                Data = new DataDictionary { { name, data } },
            };
        }

        public SqlQuery GetBool(SqlQuery sqlQuery)
            => sqlQuery;

        public string? GetColumnType(string type, PropertyInfo property)
        {
            if (driverOptions.ColumnTypes.TryGetValue(type, out var value) == true)
                return value;

            switch (type)
            {
                case "Boolean": return "BOOL";
                case "DateTime": return "DATETIME";
                case "Guid": return "CHAR(36)";
                case "Int32": return "INT";
                case "Int64": return "BIGINT";
                case "Single": return "FLOAT";
                //case "SqlGeography": return "GEOGRAPHY";
                case "String":
                    var length = property.GetCustomAttribute<MaxLengthAttribute>()?.Length
                        ?? property.GetCustomAttribute<LengthAttribute>()?.MaximumLength;

                    if (length.HasValue && length.Value > 0 && length.Value < 65536)
                        return $"VARCHAR({length.Value})";

                    return "LONGTEXT";
            }

            if (type.StartsWith("DECIMAL", StringComparison.CurrentCultureIgnoreCase))
                return type;

            throw new UnknownColumnTypeException(type);
        }

        public string GetSqlSelectedProperty(PropertyInfo property, QueryOptions options, string? defaultAlias = null)
        {
            if (driverOptions.GetSqlSelectedProperty != null)
                return driverOptions.GetSqlSelectedProperty(this, property, options, defaultAlias)
                    ?? GetColumnName(property.Name, options, defaultAlias);

            return GetColumnName(property.Name, options, defaultAlias);
        }

        public string? GetSqlColumnDefinition(PropertyInfo property)
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
                columnDefinition += " AUTO_INCREMENT";

            return columnDefinition;
        }

        public string GetJoinType(JoinType joinType)
        {
            return joinType switch
            {
                JoinType.Inner => "INNER JOIN",
                JoinType.Left => "LEFT OUTER JOIN",
                JoinType.Right => "RIGHT OUTER JOIN",
                _ => throw new UnknownJoinTypeException(joinType.ToString()),
            };
        }

        public string GetSqlOrderBy(string orderBy, QueryOptions options)
        {
            orderBy = orderBy.Trim();

            string column,
                direction;
            if (orderBy.EndsWith("ASC", true, null) && " \n\r\t".Contains(orderBy[^4]))
            {
                column = orderBy[..^4].Trim();
                direction = "ASC";
            }
            else if (orderBy.EndsWith("DESC", true, null) && " \n\r\t".Contains(orderBy[^5]))
            {
                column = orderBy[..^5].Trim();
                direction = "DESC";
            }
            else
            {
                column = orderBy;
                direction = "ASC";
            }

            column = GetColumnName(column, options);

            return $"{column} {direction}";
        }

        public IEnumerable<string> GetSqlOrderBy(IEnumerable<string> orderBy, QueryOptions options)
        {
            List<string> result = [];
            foreach (var orderByItem in orderBy)
                result.Add(GetSqlOrderBy(orderByItem, options));

            return result;
        }

        public string GetSqlLimit(QueryOptions options)
        {
            if (options.Top != null)
                return $"LIMIT {options.Offset ?? 0}, {options.Top}";

            if (options.Offset != null)
                return $"LIMIT {options.Offset}, 18446744073709551615";

            return "";
        }

        public string GetSelectLastIdQuery()
            => "SELECT LAST_INSERT_ID();";

        public string GetDataLength(string sql)
            => $"LENGTH({sql})";

        public string GetUpdateQuery(UpdateQueryOptions update)
        {
            var sql = "UPDATE " + GetTableName(update.TableName, update.Schema) + " " + GetTableAlias(update.TableAlias);
            if (!string.IsNullOrWhiteSpace(update.Joins))
                sql += " " + update.Joins;

            sql += " SET " + update.Set;

            if (!string.IsNullOrWhiteSpace(update.Where))
                sql += " WHERE " + update.Where;

            return sql;
        }
    }
}
