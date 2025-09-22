using Microsoft.Data.SqlClient;
using RFDapper;
using RFDapperDriverSQLServer.Exceptions;
using RFService.Attributes;
using RFService.Libs;
using RFService.Repo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RFDapperDriverSQLServer
{
    public class SQLServerDD(SQLServerDDOptions driverOptions)
        : IDriver
    {
        private readonly static Regex SqareBracketSingle= new(@"^\[.*\]$");
        private readonly static Regex SqareBracketDouble = new(@"^\[.*\]\.\[.*\]$");
        private readonly static Regex SqareBracketAndFree = new(@"^\[.*\]\.[\w][\w\d]*$");
        private readonly static Regex FreeAndSqareBracket = new(@"^[\w][\w\d]\.\[.*\]*$");

        public bool UseUpdateFromAlias => true;
        public bool UseUpdateSetFrom => false;

        public (DbConnection, Action) OpenConnection()
        {
            if (string.IsNullOrEmpty(driverOptions.ConnectionString))
                throw new NoConnectionStringProvidedException();

            var connection = new SqlConnection(driverOptions.ConnectionString);
            connection.Open();

            return (
                connection,
                () => connection.Dispose()
            );
        }

        public string GetDefaultSchema()
            => "[dbo]";

        public string GetSchemaName(string schema)
        {
            schema = schema.Trim();
            if (SqareBracketSingle.IsMatch(schema))
                return schema;

            if (schema.Contains('[') || schema.Contains(']'))
                throw new InvalidSchemaNameException(schema);

            var parts = schema.Split('.');
            if (parts.Length > 1)
                throw new InvalidTableNameException(schema);

            var index = parts.Length - 1;

            schema = $"[{parts[index]}]";

            return schema;
        }

        public string GetCreateSchemaIfNotExistsQuery(string schemaName)
            => $@"IF NOT EXISTS (SELECT TOP 1 1 FROM sys.schemas WHERE [name] = '{schemaName}')
                EXEC('CREATE SCHEMA {GetSchemaName(schemaName)} AUTHORIZATION {GetDefaultSchema()}');";

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
            if (SqareBracketSingle.IsMatch(tableName)
                || SqareBracketDouble.IsMatch(tableName)
                || SqareBracketAndFree.IsMatch(tableName)
                || FreeAndSqareBracket.IsMatch(tableName)
            )
                return tableName;

            if (tableName.Contains('[') || tableName.Contains(']'))
                throw new InvalidTableNameException(tableName);

            var parts = tableName.Split('.');
            if (parts.Length > 2)
                throw new InvalidTableNameException(tableName);

            var index = parts.Length - 1;

            tableName = $"[{parts[index]}]";
            index--;

            if (index >= 0)
                tableName = $"[{parts[index]}].{tableName}";
            else if (!string.IsNullOrEmpty(defaultScheme))
                tableName = $"[{defaultScheme}].{tableName}";

            return tableName;
        }

        public string GetTableAlias(string tableAlias)
        {
            tableAlias = tableAlias.Trim();
            if (SqareBracketSingle.IsMatch(tableAlias))
                return tableAlias;

            if (tableAlias.Contains('[') || tableAlias.Contains(']'))
                throw new InvalidTableAliasException(tableAlias);

            var parts = tableAlias.Split('.');
            if (parts.Length > 1)
                throw new InvalidTableAliasException(tableAlias);

            var index = parts.Length - 1;

            tableAlias = $"[{parts[index]}]";

            return tableAlias;
        }
        
        public string GetColumnAlias(string columnAlias)
        {
            columnAlias = columnAlias.Trim();
            if (SqareBracketSingle.IsMatch(columnAlias))
                return columnAlias;

            if (columnAlias.Contains('[') || columnAlias.Contains(']'))
                throw new InvalidColumnAliasException(columnAlias);

            var parts = columnAlias.Split('.');
            if (parts.Length > 1)
                throw new InvalidColumnAliasException(columnAlias);

            var index = parts.Length - 1;

            columnAlias = $"[{parts[index]}]";

            return columnAlias;
        }

        public string GetContraintName(string contraintName)
            => $"[{contraintName}]";

        public string GetClusteredQuery()
            => "CLUSTERED";

        public string GetNonClusteredQuery()
            => "NONCLUSTERED";

        public string GetCreateTableIfNotExistsQuery(string tableName, string columnsQuery, string? schemeName)
            => $"IF NOT EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'{GetTableName(tableName, schemeName)}') AND type in (N'U'))"
                + $"\r\n\tCREATE TABLE {GetTableName(tableName, schemeName)} (\r\n\t\t{columnsQuery}\r\n\t) ON [PRIMARY]";

        public string GetColumnName(string columnName, QueryOptions? options, string? defaultAlias = null)
        {
            columnName = columnName.Trim();
            if (SqareBracketSingle.IsMatch(columnName)
                || SqareBracketDouble.IsMatch(columnName)
                || SqareBracketAndFree.IsMatch(columnName)
                || FreeAndSqareBracket.IsMatch(columnName)
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
            else if (!string.IsNullOrEmpty(options?.Alias))
                columnName = $"[{options.Alias}].{columnName}";

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
        {
            return new()
            {
                Sql = "CASE WHEN " + sqlQuery.Sql + " THEN 1 ELSE 0 END",
                Data = sqlQuery.Data,
            };
        }

        public string? GetColumnType(string type, PropertyInfo property)
        {
            if (driverOptions.ColumnTypes.TryGetValue(type, out var value) == true)
                return value;

            switch (type)
            {
                case "Boolean": return "BIT";
                case "DateTime": return "DATETIME";
                case "Guid": return "UNIQUEIDENTIFIER";
                case "Int32": return "INT";
                case "Int64": return "BIGINT";
                case "Single": return "FLOAT";
                case "SqlGeography": return "GEOGRAPHY";

                case "String":
                    {
                        var length = property.GetCustomAttribute<MaxLengthAttribute>()?.Length
                            ?? property.GetCustomAttribute<LengthAttribute>()?.MaximumLength
                            ?? property.GetCustomAttribute<SizeAttribute>()?.Size;

                        if (length.HasValue && length.Value > 0)
                            return $"NVARCHAR({length.Value})";

                        return "NVARCHAR(MAX)";
                    }

                case "Byte[]":
                    {
                        var length = property.GetCustomAttribute<MaxLengthAttribute>()?.Length
                            ?? property.GetCustomAttribute<LengthAttribute>()?.MaximumLength
                            ?? property.GetCustomAttribute<SizeAttribute>()?.Size;

                        if (length.HasValue && length.Value > 0)
                            return $"VARBINARY({length.Value})";

                        return "VARBINARY(MAX)";
                    }
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
                columnDefinition += " IDENTITY(1,1)";

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

            if (orderBy.Any())
            {
                foreach (var orderByItem in orderBy)
                    result.Add(GetSqlOrderBy(orderByItem, options));
            }
            else if (options.Top != null || options.Offset != null)
            {
                result.Add(GetSqlOrderBy("Id", options));
            }

            return result;
        }

        public string GetSqlLimit(QueryOptions options)
        {
            if (options.Top != null)
                return $"OFFSET {options.Offset ?? 0} ROWS FETCH NEXT {options.Top} ROWS ONLY";

            if (options.Offset != null)
                return $"OFFSET {options.Offset} ROWS";

            return "";
        }

        public string GetSelectLastIdQuery()
            => ";SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

        public string GetDataLength(string sql)
        {
            return $"DATALENGTH({sql})";
        }
    }
}
