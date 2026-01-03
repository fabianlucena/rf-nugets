using Npgsql;
using RFDapper;
using RFDapperDriverPostgreSQL.Exceptions;
using RFOperators;
using RFService.Attributes;
using RFService.Libs;
using RFService.Repo;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RFDapperDriverPostgreSQL
{
    public partial class PostgreSQLDD
        : IDriver
    {
        [GeneratedRegex("^\".*\"$")]
        private static partial Regex QuotedSingleConstructor();
        private readonly static Regex QuotedSingle = QuotedSingleConstructor();

        [GeneratedRegex("^\".*\"\\.\".*\"$")]
        private static partial Regex QuotedDoubleConstructor();
        private readonly static Regex QuotedDouble = QuotedDoubleConstructor();

        [GeneratedRegex("^\".*\"\\.[\\w][\\w\\d]*$")]
        private static partial Regex QuotedAndFreeConstructor();
        private readonly static Regex QuotedAndFree = QuotedAndFreeConstructor();

        [GeneratedRegex("^[\\w][\\w\\d]\\.\".*\"*$")]
        private static partial Regex FreeAndQuotedConstructor();
        private readonly static Regex FreeAndQuoted = FreeAndQuotedConstructor();

        PostgreSQLDDOptions DriverOptions { get; set; }

        public PostgreSQLDD(PostgreSQLDDOptions driverOptions)
        {
            DriverOptions = driverOptions;
            if (DriverOptions.DataSource == null)
            {
                if (string.IsNullOrEmpty(DriverOptions.ConnectionString))
                    throw new NoConnectionStringProvidedException();

                var dsb = new NpgsqlDataSourceBuilder(DriverOptions.ConnectionString);
                DriverOptions.PrepareDataSourceBuilder?.Invoke(dsb);
                DriverOptions.DataSource = dsb.Build();
            }
        }

        public (DbConnection, Action) OpenConnection()
        {
            var connection = DriverOptions.DataSource!.OpenConnection();
            return (
                connection,
                async () => await connection.DisposeAsync()
            );
        }

        public string GetDefaultSchema()
            => "\"public\"";

        public string GetSchemaName(string schema)
        {
            schema = schema.Trim();
            if (QuotedSingle.IsMatch(schema))
                return schema;

            if (schema.Contains('"') || schema.Contains('"'))
                throw new InvalidSchemaNameException(schema);

            var parts = schema.Split('.');
            if (parts.Length > 1)
                throw new InvalidTableNameException(schema);

            var index = parts.Length - 1;

            schema = $"\"{parts[index]}\"";

            return schema;
        }

        public string GetCreateSchemaIfNotExistsQuery(string schemaName)
            => $"CREATE SCHEMA IF NOT EXISTS {GetSchemaName(schemaName)};"; 
        
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
            if (QuotedSingle.IsMatch(tableName)
                || QuotedDouble.IsMatch(tableName)
                || QuotedAndFree.IsMatch(tableName)
                || FreeAndQuoted.IsMatch(tableName)
            )
                return tableName;

            if (tableName.Contains('"') || tableName.Contains('"'))
                throw new InvalidTableNameException(tableName);

            var parts = tableName.Split('.');
            if (parts.Length > 2)
                throw new InvalidTableNameException(tableName);

            var index = parts.Length - 1;

            tableName = $"\"{parts[index]}\"";
            index--;

            if (index >= 0)
                tableName = $"\"{parts[index]}\".{tableName}";
            else if (!string.IsNullOrEmpty(defaultScheme))
                tableName = $"\"{defaultScheme}\".{tableName}";

            return tableName;
        }

        public string GetTableAlias(string tableAlias)
        {
            tableAlias = tableAlias.Trim();
            if (QuotedSingle.IsMatch(tableAlias))
                return tableAlias;

            if (tableAlias.Contains('"') || tableAlias.Contains('"'))
                throw new InvalidTableAliasException(tableAlias);

            var parts = tableAlias.Split('.');
            if (parts.Length > 1)
                throw new InvalidTableAliasException(tableAlias);

            var index = parts.Length - 1;

            tableAlias = $"\"{parts[index]}\"";

            return tableAlias;
        }
        
        public string GetColumnAlias(string columnAlias)
        {
            columnAlias = columnAlias.Trim();
            if (QuotedSingle.IsMatch(columnAlias))
                return columnAlias;

            if (columnAlias.Contains('"') || columnAlias.Contains('"'))
                throw new InvalidColumnAliasException(columnAlias);

            var parts = columnAlias.Split('.');
            if (parts.Length > 1)
                throw new InvalidColumnAliasException(columnAlias);

            var index = parts.Length - 1;

            columnAlias = $"\"{parts[index]}\"";

            return columnAlias;
        }

        public string GetContraintName(string contraintName)
            => $"\"{contraintName}\"";

        public string GetClusteredQuery()
            => "CLUSTERED";

        public string GetNonClusteredQuery()
            => "";

        public string GetColumnName(string columnName, QueryOptions? options, string? defaultAlias = null)
        {
            columnName = columnName.Trim();
            if (QuotedSingle.IsMatch(columnName)
                || QuotedDouble.IsMatch(columnName)
                || QuotedAndFree.IsMatch(columnName)
                || FreeAndQuoted.IsMatch(columnName)
            )
                return columnName;

            if (columnName.Contains('"') || columnName.Contains('"'))
                throw new InvalidColumnNameException(columnName);

            var parts = columnName.Split('.');
            if (parts.Length > 2)
                throw new InvalidColumnNameException(columnName);

            var index = parts.Length - 1;

            columnName = $"\"{parts[index]}\"";
            index--;

            if (index >= 0)
                columnName = $"\"{parts[index]}\".{columnName}";
            else if (!string.IsNullOrEmpty(defaultAlias))
                columnName = $"\"{defaultAlias}\".{columnName}";
            else if (!string.IsNullOrEmpty(options?.Alias))
                columnName = $"\"{options.Alias}\".{columnName}";

            return columnName;
        }

        public string GetCreateTableIfNotExistsQuery(string tableName, string columnsQuery, string? schemeName)
            => $"CREATE TABLE IF NOT EXISTS {GetTableName(tableName, schemeName)} (\r\n\t\t{columnsQuery}\r\n\t)";

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
            if (DriverOptions.ColumnTypes.TryGetValue(type, out var getColumnType) == true)
            {
                var value = getColumnType(property);
                if (!string.IsNullOrEmpty(value))
                    return value;
            }

            switch (type)
            {
                case "Boolean": return "BOOLEAN";
                case "DateTime": return "TIMESTAMP";
                case "Guid": return "UUID";
                case "Int32": return "INT";
                case "Int64":
                    var databaseGeneratedAttribute = property.GetCustomAttribute<DatabaseGeneratedAttribute>();
                    if (databaseGeneratedAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                        return "BIGSERIAL";
                    return "BIGINT";

                case "Single": return "FLOAT";

                case "String":
                    {
                        var length = property.GetCustomAttribute<MaxLengthAttribute>()?.Length
                            ?? property.GetCustomAttribute<LengthAttribute>()?.MaximumLength
                            ?? property.GetCustomAttribute<SizeAttribute>()?.Size;

                        if (length.HasValue && length.Value > 0)
                            return $"VARCHAR({length.Value})";

                        return "TEXT";
                    }

                case "Byte[]": return "BYTEA";
            }

            if (type.StartsWith("DECIMAL", StringComparison.CurrentCultureIgnoreCase))
                return type;

            throw new UnknownColumnTypeException(type);
        }

        public string GetSqlSelectedProperty(PropertyInfo property, QueryOptions options, string? defaultAlias = null)
        {
            if (DriverOptions.GetSqlSelectedProperty != null)
                return DriverOptions.GetSqlSelectedProperty(this, property, options, defaultAlias)
                    ?? GetColumnName(property.Name, options, defaultAlias);

            return GetColumnName(property.Name, options, defaultAlias);
        }

        public string? GetSqlColumnDefinition(PropertyInfo property)
        {
            var propertyType = property.PropertyType;
            var columnAttributes = property.GetCustomAttribute<ColumnAttribute>();
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

            var columnDefinition = $"\"{property.Name}\" {columnType}";

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

        public string GetReturnInsertedIdQuery()
            => "RETURNING \"Id\"";

        public string GetDataLength(string sql)
            => $"DATALENGTH({sql})";

        public SqlQuery? GetOperation(Operator op, QueryOptions options, List<string> usedNames, string paramName,
            Func<Operator, QueryOptions, List<string>, string, SqlQuery> getOperation)
        {
            if (op is not Binary bop)
                return null;

            if (op is not In && op is not NotIn)
                return null;

            var sqlQuery = new SqlQuery { Precedence = op.Precedence };
            var sqlQuery1 = getOperation(bop.Op1, options, usedNames, paramName);
            var sqlQuery2 = getOperation(bop.Op2, options, usedNames, paramName);

            var sql1 = sqlQuery1.Sql;
            if (sqlQuery1.Precedence > sqlQuery.Precedence)
                sql1 = $"({sql1})";

            var sql2 = sqlQuery2.Sql;
            if (sqlQuery2.Precedence > sqlQuery.Precedence)
                sql2 = $"({sql2})";

            if (op is In)
                sqlQuery.Sql = $"{sql1} = ANY ({sql2})";
            else if (op is NotIn)
                sqlQuery.Sql = $"{sql1} <> ALL ({sql2})";
            else
                return null;

            foreach (var kv in sqlQuery1.Data)
                sqlQuery.Data[kv.Key] = ConvertToList(kv.Value) ?? kv.Value;

            foreach (var kv in sqlQuery2.Data)
                sqlQuery.Data[kv.Key] = ConvertToList(kv.Value) ?? kv.Value;

            return sqlQuery;
        }

        static public object? ConvertToList(object? value)
        {
            if (value is not null && value is IEnumerable enumerable)
            {
                if (value.GetType().IsGenericType &&
                    value.GetType().GetGenericTypeDefinition() == typeof(List<>))
                {
                    return null;
                }

                var enumType = value.GetType();
                var enumerableInterface = enumType
                    .GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                if (enumerableInterface == null)
                    return null;

                var itemType = enumerableInterface.GetGenericArguments()[0];
                var listType = typeof(List<>).MakeGenericType(itemType);
                var constructor = listType.GetConstructor([enumerableInterface]);
                if (constructor == null)
                    return null;

                return constructor.Invoke([value]);
            }

            return null;
        }

        public string GetUpdateQuery(UpdateQueryOptions update)
        {
            var sql = "UPDATE " + GetTableName(update.TableName, update.Schema) + " " + GetTableAlias(update.TableAlias)
                + " SET " + update.Set;

            if (!string.IsNullOrWhiteSpace(update.TruncatedJoins))
                sql += " " + update.TruncatedJoins;

            if (!string.IsNullOrWhiteSpace(update.Where))
            {
                if (!string.IsNullOrWhiteSpace(update.FirstJoinCondition))
                    sql += " WHERE (" + update.FirstJoinCondition + ") AND (" + update.Where + ")";
                else
                    sql += " WHERE " + update.Where;
            }
            else if (!string.IsNullOrWhiteSpace(update.FirstJoinCondition))
                sql += " WHERE " + update.FirstJoinCondition;

            return sql;
        }
    }
}
