using Dapper;
using Microsoft.Extensions.Logging;
using RFDapper.Exceptions;
using RFOperators;
using RFService.Attributes;
using RFService.ILibs;
using RFService.IRepo;
using RFService.Libs;
using RFService.Repo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text.Json;

namespace RFDapper
{
    public class SqlQueryParts
    {
        public string SqlSelect = "";

        public string SqlFrom = "";

        public DataDictionary Data = [];
    }

    public class Dapper<TEntity>
        : IRepo<TEntity>
        where TEntity : class
    {
        private readonly ILogger<Dapper<TEntity>> _logger;
        private readonly IDriver _driver;
        private readonly string _tableName;
        private readonly string? _schema = null;

        public ILogger<Dapper<TEntity>> Logger { get => _logger; }
        public string TableName { get => _tableName; }
        public string Schema { get => _schema ?? _driver.GetDefaultSchema(); }

        public Dapper(
            ILogger<Dapper<TEntity>> logger,
            IDriver driver
        )
        {
            _logger = logger;
            _driver = driver;
            var tableAttribute = typeof(TEntity).GetCustomAttribute<TableAttribute>();
            if (tableAttribute == null)
            {
                _tableName = typeof(TEntity).Name;
            }
            else
            {
                _tableName = tableAttribute.Name;
                if (!string.IsNullOrEmpty(tableAttribute.Schema))
                {
                    _schema = tableAttribute.Schema;
                }
            }
        }

        public (IDbConnection, Action) OpenConnection(RepoOptions? options = null)
        {
            if (options?.Connection != null)
                return (options.Connection, () => { });

            return _driver.OpenConnection();
        }

        public void CreateTable()
        {
            var query = _driver.GetCreateSchemaIfNotExistsQuery(Schema);
            var (connection, close) = _driver.OpenConnection();
            try
            {
                connection.Query(query);

                var entityType = typeof(TEntity);
                var properties = entityType.GetProperties();
                var columnsQueries = new List<string>();
                var postQueries = new List<string>();
                foreach (var property in properties)
                {
                    if (IsVirtual(property)
                        || GetForeignColumnName(typeof(TEntity), property.Name) != null
                    )
                        continue;

                    var columnDefinition = _driver.GetSqlColumnDefinition(property);

                    if (columnDefinition != null)
                    {
                        columnsQueries.Add(columnDefinition);

                        var settedPk = false;
                        var databaseGeneratedAttribute = property.GetCustomAttribute<DatabaseGeneratedAttribute>();
                        if (databaseGeneratedAttribute != null)
                        {
                            if (databaseGeneratedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                            {
                                postQueries.Add($"CONSTRAINT {_driver.GetContraintName($"{Schema}_{TableName}_PK")} PRIMARY KEY {_driver.GetNonClusteredQuery()} ({_driver.GetColumnName(property.Name)})");
                                settedPk = true;
                            }
                        }

                        if (!settedPk && property.GetCustomAttribute<KeyAttribute>() != null)
                            postQueries.Add($"CONSTRAINT {_driver.GetContraintName($"{Schema}_{TableName}_PK_{property.Name}")} PRIMARY KEY {_driver.GetClusteredQuery()} ({_driver.GetColumnName(property.Name)})");
                    }

                    var foreign = property.GetCustomAttribute<ForeignKeyAttribute>();
                    if (foreign != null)
                    {
                        var foreignObject = entityType.GetProperty(foreign.Name) ??
                            throw new Exception($"Unknown foreign {foreign.Name}");

                        var foreignObjectType = foreignObject.PropertyType;
                        var referenceColumn = property;
                        if (!foreignObjectType.IsClass)
                        {
                            var propertyType = property.PropertyType;
                            if (!propertyType.IsClass)
                                throw new Exception($"Foreign is not and object{foreign.Name}");

                            referenceColumn = foreignObject;
                            foreignObject = property;

                            foreignObjectType = propertyType;
                        }

                        var foreignTableAttribute = foreignObjectType.GetCustomAttribute<TableAttribute>();
                        string foreignTable = "",
                            foreignSchema = _driver.GetDefaultSchema(),
                            foreignColumn = "Id";
                        if (foreignTableAttribute == null)
                            foreignTable = typeof(TEntity).Name;
                        else
                        {
                            foreignTable = foreignTableAttribute.Name;
                            if (!string.IsNullOrEmpty(foreignTableAttribute.Schema))
                                foreignSchema = foreignTableAttribute.Schema;
                        }

                        postQueries.Add($"CONSTRAINT {_driver.GetContraintName($"{Schema}_{TableName}_{referenceColumn.Name}_FK_{foreignSchema}_{foreignTable}_{foreignColumn}")}"
                            + $" FOREIGN KEY({_driver.GetColumnName(referenceColumn.Name)}) REFERENCES {_driver.GetTableName(foreignTable, foreignSchema)} ({_driver.GetColumnName(foreignColumn)})"
                        );
                    }
                }

                var indexes = entityType.GetCustomAttributes<IndexAttribute>();
                if (indexes != null)
                {
                    foreach (var index in indexes)
                    {
                        var name = index.Name ?? $"{Schema}_{TableName}_{(index.IsUnique ? "U" : "I")}K_{string.Join('_', index.PropertyNames)}";
                        var indexType = index.IsUnique ?
                            "UNIQUE" :
                            "INDEX";

                        postQueries.Add($"CONSTRAINT {_driver.GetContraintName(name)} {indexType} ({string.Join(", ", index.PropertyNames.Select(p => _driver.GetColumnName(p)))})");
                    }
                }

                var columnsQuery = string.Join(",\r\n\t\t", [.. columnsQueries]);
                if (postQueries.Count > 0)
                    columnsQuery += ",\r\n\t\t" + string.Join(",\r\n\t\t", [.. postQueries]);

                query = _driver.GetCreateTableIfNotExistsQuery(TableName, columnsQuery, Schema);

                _logger.LogDebug("{query}", query);
                connection.Query(query);
            }
            finally
            {
                close();
            }
        }

        public string? GetPrimaryKey()
        {
            var entityType = typeof(TEntity);
            var properties = entityType.GetProperties();
            foreach (var property in properties)
            {
                var databaseGeneratedAttribute = property.GetCustomAttribute<DatabaseGeneratedAttribute>();
                if (databaseGeneratedAttribute == null)
                    continue;

                if (databaseGeneratedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                    return property.Name;

                if (property.GetCustomAttribute<KeyAttribute>() != null)
                    return property.Name;
            }

            return null;
        }

        public SqlQuery GetOperation(
            Operator op,
            QueryOptions options,
            List<string> usedNames,
            string paramName
        )
        {
            var sqlQuery = _driver.GetOperation(op, options, usedNames, paramName, GetOperation);
            if (sqlQuery != null)
                return sqlQuery;

            sqlQuery ??= new SqlQuery { Precedence = op.Precedence };

            if (op is Column col)
            {
                sqlQuery.Sql = _driver.GetColumnName(col.Name, options);
                return sqlQuery;
            }

            if (op is Value val)
            {
                if (val.Data == null)
                {
                    sqlQuery.IsNull = true;
                    return sqlQuery;
                }

                sqlQuery = _driver.GetValue(val.Data, options, usedNames, paramName);
                sqlQuery.Precedence = op.Precedence;
                return sqlQuery;
            }

            if (op is Unary uop)
            {
                var innerSqlQuery = GetOperation(uop.Op, options, usedNames, paramName);
                var sql = innerSqlQuery.Sql;
                if (innerSqlQuery.Precedence > sqlQuery.Precedence)
                    sql = $"({sql})";

                sqlQuery.Sql = op switch
                {
                    IsNull => $"{sql} IS NULL",
                    IsNotNull => $"{sql} IS NOT NULL",
                    Not => $"NOT {sql}",
                    MakeValid => $"{sql}.MakeValid()",
                    Sum => $"SUM({sql})",
                    DataLength => _driver.GetDataLength(sql),
                    _ => throw new UnknownUnaryOperatorException(op.GetType().Name),
                };

                foreach (var kv in innerSqlQuery.Data)
                    sqlQuery.Data[kv.Key] = kv.Value;

                return sqlQuery;
            }

            if (op is Binary bop)
            {
                var sqlQuery1 = GetOperation(bop.Op1, options, usedNames, paramName);
                var sql1 = sqlQuery1.Sql;
                if (sqlQuery1.Precedence > sqlQuery.Precedence)
                    sql1 = $"({sql1})";

                var sqlQuery2 = GetOperation(bop.Op2, options, usedNames, paramName);
                var sql2 = sqlQuery2.Sql;
                if (sqlQuery2.Precedence > sqlQuery.Precedence)
                    sql2 = $"({sql2})";

                sqlQuery.Sql = op switch
                {
                    Eq => sqlQuery2.IsNull ?
                        $"{sql1} IS NULL" :
                        $"{sql1} = {sql2}",
                    NE => sqlQuery2.IsNull ?
                        $"{sql1} IS NOT NULL" :
                        $"{sql1} <> {sql2}",
                    In => $"{sql1} IN {sql2}",
                    NotIn => $"{sql1} NOT IN {sql2}",
                    ST_Intersects => GetSTIntersectsQuery(sqlQuery1, sqlQuery2),
                    ST_Contains => GetSTContainsQuery(sqlQuery1, sqlQuery2),
                    Like => $"{sql1} LIKE {sql2}",
                    NotLike => $"{sql1} NOT LIKE {sql2}",
                    GT => $"{sql1} > {sql2}",
                    GE => $"{sql1} >= {sql2}",
                    LT => $"{sql1} < {sql2}",
                    LE => $"{sql1} <= {sql2}",
                    Add => $"{sql1} + {sql2}",
                    _ => throw new UnknownBinaryOperatorException(op.GetType().Name),
                };

                foreach (var kv in sqlQuery1.Data)
                    sqlQuery.Data[kv.Key] = kv.Value;

                foreach (var kv in sqlQuery2.Data)
                    sqlQuery.Data[kv.Key] = kv.Value;

                return sqlQuery;
            }

            if (op is NAry nop)
            {
                List<string> sqls = [];

                foreach (var iop in nop.Ops)
                {
                    var newSqlQuery = GetOperation(iop, options, usedNames, paramName);
                    var newSql = newSqlQuery.Sql;
                    if (newSqlQuery.Precedence > sqlQuery.Precedence)
                        newSql = $"({newSql})";

                    sqls.Add(newSql);
                    foreach (var kv in newSqlQuery.Data)
                        sqlQuery.Data[kv.Key] = kv.Value;
                }

                sqlQuery.Sql = op switch
                {
                    And => string.Join(" AND ", sqls),
                    Or => string.Join(" OR ", sqls),
                    _ => throw new UnknownNAryOperatorException(op.GetType().Name),
                };

                return sqlQuery;
            }

            throw new UnknownOperationException(op.GetType().Name);
        }

        private readonly Access AccessOperator = new(new Value(null), new Value(null));

        public string GetSTIntersectsQuery(SqlQuery sqlQuery1, SqlQuery sqlQuery2)
        {
            var sql1 = sqlQuery1.Sql;
            if (sqlQuery1.Precedence > AccessOperator.Precedence)
                sql1 = $"({sql1})";

            return $"{sql1}.STIntersects({sqlQuery2.Sql}) = 1";
        }

        public string GetSTContainsQuery(SqlQuery sqlQuery1, SqlQuery sqlQuery2)
        {
            var sql1 = sqlQuery1.Sql;
            if (sqlQuery1.Precedence > AccessOperator.Precedence)
                sql1 = $"({sql1})";

            return $"{sql1}.STContains({sqlQuery2.Sql}) = 1";
        }

        public SqlQuery GetFilterQuery(
            Operators operators,
            QueryOptions options,
            List<string> usedNames,
            string paramName
        )
        {
            if (operators.Count <= 0)
                return new SqlQuery();

            if (operators.Count == 1)
                return GetOperation(operators[0], options, usedNames, paramName);

            return GetOperation(Op.And([.. operators]), options, usedNames, paramName);
        }

        public SqlQuery? GetWhereFilter(QueryOptions options, List<string>? usedNames, string paramName)
        {
            if (options.Filters.Count == 0)
                return null;

            var sqlQuery = GetFilterQuery(options.Filters, options, usedNames ?? [], paramName);

            return sqlQuery;
        }

        public SqlQuery? GetWhereQuery(QueryOptions options, List<string>? usedNames, string paramName)
        {
            var filter = GetWhereFilter(options, usedNames, paramName);
            if (filter != null)
            {
                filter.SqlNoCommand = filter.Sql;
                filter.Sql = "WHERE " + filter.Sql;
            }

            return filter;
        }

        private string? GetTableNameForEntity(Type entity)
        {
            if (entity == null)
                return null;

            var tableAttribute = entity.GetCustomAttribute<TableAttribute>();
            if (tableAttribute == null)
                return null;

            return _driver.GetTableName(tableAttribute.Name, tableAttribute.Schema);
        }

        private static bool IsVirtual(PropertyInfo property)
            => property.GetCustomAttribute<VirtualAttribute>() != null;

        private static string? GetForeignColumnName(Type entityType, string propertyName)
        {
            var properties = entityType.GetProperties();
            foreach (var property in properties)
            {
                var foreign = property.GetCustomAttribute<ForeignKeyAttribute>();
                if (foreign == null || foreign.Name != propertyName)
                    continue;

                return property.Name;
            }

            return null;
        }

        private static bool IsForeignColumnNullable(string? propertyName)
        {
            var entityType = typeof(TEntity);
            var properties = entityType.GetProperties();
            foreach (var property in properties)
            {
                var foreign = property.GetCustomAttribute<ForeignKeyAttribute>();
                if (foreign == null || foreign.Name != propertyName)
                    continue;

                var propertyType = property.PropertyType;
                return propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            return false;
        }

        public List<string> GetSelectedColumns(
            Type entityType,
            IDriver driver,
            QueryOptions options,
            List<string> usedNames,
            string paramName,
            string? defaultAlias = null
        )
        {
            var columns = new List<string>();

            if (options.Select != null)
            {
                foreach (var operation in options.Select)
                {
                    SqlQuery sqlQuery;
                    if (operation is As withAlias)
                    {
                        sqlQuery = GetOperation(withAlias.Op, options, usedNames, paramName);
                        sqlQuery.Sql += " AS " + driver.GetColumnAlias(withAlias.Alias);
                    }
                    else
                    {
                        sqlQuery = GetOperation(operation, options, usedNames, paramName);
                    }

                    columns.Add(sqlQuery.Sql);
                }
            }
            else
            {
                var properties = entityType.GetProperties();
                foreach (var property in properties)
                {
                    if (GetForeignColumnName(entityType, property.Name) != null)
                        continue;

                    if (IsVirtual(property))
                        continue;

                    var column = driver.GetSqlSelectedProperty(property, options, defaultAlias);
                    columns.Add(column);
                }
            }

            return columns;
        }

        public (string, DataDictionary, string, string) GetJoinQuery(
            QueryOptions options,
            List<string> usedNames,
            List<string>? sqlColumns = null
        )
        {
            string join = "";
            string truncateJoin = "";
            DataDictionary data = [];
            bool firstJoin = true;
            string firstJoinCondition = "";
            foreach (var from in options.Join)
            {
                Type? propertyType = string.IsNullOrEmpty(from.PropertyName) ?
                    null :
                    typeof(TEntity)?.GetProperty(from.PropertyName)?.PropertyType;

                Type entity = from.Entity
                    ?? propertyType
                    ?? throw new NoEntityForJoinException();

                var joinType = from.Type
                    ?? (IsForeignColumnNullable(from.PropertyName) ? JoinType.Left : JoinType.Inner);

                if (string.IsNullOrEmpty(from.Alias))
                    from.Alias = options.CreateAlias("t");

                Operator? onOperation = from.On;
                if (onOperation == null)
                {
                    if (string.IsNullOrWhiteSpace(from.PropertyName))
                        throw new NoOnClauseForJoinWithEntityException();

                    onOperation = Op.Eq(
                        Op.Column($"{from.Alias}.Id"),
                        Op.Column($"{options.Alias}.{GetForeignColumnName(typeof(TEntity), from.PropertyName)}")
                    );
                }

                SqlQuery? sqlQuery = GetOperation(onOperation, options, usedNames, "select_param");

                if (sqlColumns != null)
                {
                    if (!string.IsNullOrWhiteSpace(from.PropertyName))
                    {
                        sqlColumns.Add("NULL AS " + _driver.GetColumnAlias(options.Separator));
                        sqlColumns.AddRange(GetSelectedColumns(entity, _driver, options, usedNames, "select_param", from.Alias));
                    }
                }

                var thisJoin = $" {_driver.GetJoinType(joinType)} {GetTableNameForEntity(entity)} {_driver.GetTableAlias(from.Alias)}"
                    + $" ON {sqlQuery.Sql}";

                if (firstJoin)
                {
                    join = thisJoin;
                    truncateJoin = $"{GetTableNameForEntity(entity)} {_driver.GetTableAlias(from.Alias)}";
                    firstJoinCondition = sqlQuery.Sql;
                    firstJoin = false;
                }
                else
                {
                    join += thisJoin;
                    truncateJoin += thisJoin;
                }

                foreach (var value in sqlQuery.Data)
                    data.Add(value.Key, value.Value);
            }

            return (join, data, truncateJoin, firstJoinCondition);
        }

        public SqlQueryParts GetSelectQueryParts(QueryOptions options)
        {
            options = new QueryOptions(options);
            options.Alias = options.GetOrCreateAlias("t");

            List<string> usedNames = [];
            var sqlColumns = GetSelectedColumns(typeof(TEntity), _driver, options, usedNames, "select_param");
            var sqlFrom = $" FROM {_driver.GetTableName(TableName, Schema)} {_driver.GetTableAlias(options.Alias)}";
            SqlQuery? sqlQuery;

            DataDictionary data = [];
            if (options != null)
            {
                (string joins, DataDictionary joinData, _, _) = GetJoinQuery(
                    options,
                    usedNames,
                    options.Select == null ? sqlColumns : null
                );

                sqlFrom += joins;
                foreach (var value in joinData)
                    data.Add(value.Key, value.Value);

                sqlQuery = GetWhereQuery(options, usedNames, "where_param");
                if (!string.IsNullOrWhiteSpace(sqlQuery?.Sql))
                {
                    sqlFrom += " " + sqlQuery.Sql;
                    foreach (var value in sqlQuery.Data)
                        data.Add(value.Key, value.Value);
                }

                var orderBy = new List<string>(options.OrderBy);
                if (options.OrderBy.Count == 0 && (options.Offset != null || options.Top != null))
                {
                    var newOrderBy = GetPrimaryKey();
                    if (string.IsNullOrEmpty(newOrderBy))
                    {
                        var entityType = typeof(TEntity);
                        var properties = entityType.GetProperties();
                        newOrderBy = properties[0].Name;
                    }

                    orderBy.Add($"{options.Alias ?? TableName}.{newOrderBy}");
                }

                if (orderBy.Count > 0)
                    sqlFrom += $" ORDER BY {string.Join(", ", _driver.GetSqlOrderBy(orderBy, options))}";

                if (options.Offset != null || options.Top != null)
                    sqlFrom += " " + _driver.GetSqlLimit(options);
            }

            return new SqlQueryParts
            {
                SqlSelect = "SELECT " + string.Join(',', sqlColumns),
                SqlFrom = sqlFrom,
                Data = data,
            };
        }

        public SqlQuery GetSelectQuery(QueryOptions options)
        {
            var parts = GetSelectQueryParts(options);

            return new SqlQuery
            {
                Sql = parts.SqlSelect + parts.SqlFrom,
                Data = parts.Data,
            };
        }

        public SqlQuery GetInsertQuery(TEntity data)
        {
            var entityType = typeof(TEntity);
            var properties = data.GetType().GetProperties();
            var columns = new List<string>();
            var valuesName = new List<string>();
            var newData = new DataDictionary();
            bool hasId = false;

            foreach (var p in properties)
            {
                string name = p.Name;
                if (name == "Id")
                {
                    hasId = true;
                    continue;
                }

                var property = entityType.GetProperty(name) ??
                    throw new Exception($"Unknown {name} property");

                if (IsVirtual(property))
                    continue;

                var propertyType = property.PropertyType;
                var value = property.GetValue(data, null);
                if (propertyType.IsClass
                    && propertyType.Name != "String"
                    && GetForeignColumnName(typeof(TEntity), name) != null
                )
                    continue;

                var varName = "@" + name;
                columns.Add(_driver.GetColumnName(name));
                valuesName.Add(varName);
                newData[varName] = value;
            }

            var sql = $"INSERT INTO {_driver.GetTableName(TableName, Schema)} ({string.Join(",", columns)}) VALUES ({string.Join(",", valuesName)})";
            if (hasId)
                sql += _driver.GetReturnInsertedIdQuery();

            return new SqlQuery
            {
                Sql = sql,
                Data = newData,
            };
        }

        public static object? ConvertJsonElement(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    var dict = new DataDictionary();
                    foreach (var prop in element.EnumerateObject())
                    {
                        dict[prop.Name] = ConvertJsonElement(prop.Value);
                    }
                    return dict;
                
                case JsonValueKind.Array:
                    var list = new List<object?>();
                    foreach (var item in element.EnumerateArray())
                    {
                        list.Add(ConvertJsonElement(item));
                    }
                    return list;
                
                case JsonValueKind.String:
                    return element.GetString();
                
                case JsonValueKind.Number:
                    if (element.TryGetInt32(out int intValue))
                        return intValue;
                    
                    if (element.TryGetInt64(out long longValue))
                        return longValue;
                    
                    if (element.TryGetDouble(out double doubleValue))
                        return doubleValue;
                    
                    break;
                
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return element.GetBoolean();
                
                case JsonValueKind.Null:
                    return null;
            }

            return element.GetRawText();
        }

        public SqlQuery GetUpdateQuery(
            IDataDictionary data,
            QueryOptions options
        )
        {
            options.Alias = options.GetOrCreateAlias("t");
            var sqlWhere = GetWhereQuery(options, [], "where_param")
                ?? throw new Exception("UPDATE without WHERE is forbidden");

            var entityType = typeof(TEntity);
            var columns = new List<string>();
            DataDictionary values = [];
            List<string> usedNames = [];

            foreach (var item in data)
            {
                var name = item.Key;
                var property = entityType.GetProperty(name);
                if (property == null)
                    continue;

                var value = item.Value;
                if (value != null)
                {
                    JsonElement? valueJsonElement = value as JsonElement?;
                    if (valueJsonElement != null)
                        value = ConvertJsonElement(valueJsonElement.Value);

                    if (value is Operator op) {
                        var operation = GetOperation(op, options, usedNames, "data_" + name);
                        foreach (var dataItem in operation.Data)
                            values[dataItem.Key] = dataItem.Value;

                        if (property.PropertyType == typeof(bool))
                            operation = _driver.GetBool(operation);

                        columns.Add($"{_driver.GetColumnName(name)}={operation.Sql}");
                        continue;
                    }
                }
                var valueName = "data_" + name;
                values[valueName] = value;
                columns.Add($"{_driver.GetColumnName(name)}=@{valueName}");
            }

            if (columns.Count <= 0)
                throw new NothingToUpdateException();

            options.Alias = options.GetOrCreateAlias("t");

            (string joins, DataDictionary joinData, string truncatedJoins, string firstJoinCondition) = GetJoinQuery(
                options,
                usedNames
            );

            var sql = _driver.GetUpdateQuery(new UpdateQueryOptions {
                Schema = Schema,
                TableName = TableName,
                TableAlias = options.Alias,
                Set = string.Join(",", columns),
                Joins = joins,
                Where = sqlWhere.SqlNoCommand,
                TruncatedJoins = truncatedJoins,
                FirstJoinCondition = firstJoinCondition
            });

            foreach (var value in joinData)
                data.Add(value.Key, value.Value);

            return new SqlQuery
            {
                Sql = sql,
                Data = values.Concat(sqlWhere.Data).ToDataDictionary(),
            };
        }

        public SqlQuery GetDeleteQuery(QueryOptions options)
        {
            var sqlWhere = GetWhereQuery(options, [], "where_param")
                ?? throw new Exception("DELETE without WHERE is forbidden");

            DataDictionary values = [];

            var sql = $"DELETE FROM {_driver.GetTableName(TableName, Schema)} {sqlWhere.Sql}";
            return new SqlQuery
            {
                Sql = sql,
                Data = values.Concat(sqlWhere.Data).ToDataDictionary(),
            };
        }

        static void SetId(TEntity data, long id)
        {
            var type = data.GetType();
            var pId = type.GetProperty("Id");
            pId?.SetValue(data, id);
        }

        static public bool IsValidObject<T>(T data, bool throwException = true)
        {
            if (data == null)
            {
                if (throwException)
                    throw new InvalidObjectBecauseIsNullException();

                return false;
            }

            var type = data.GetType();
            var pId = type.GetProperty("Id");
            if (pId == null)
                return true;

            var oId = pId.GetValue(data);
            if (oId is not Int64 id)
            {
                if (throwException)
                    throw new InvalidObjectBecauseTheIdIsNotALongValueException();

                return false;
            }

            return id != 0;
        }

        public async Task<TEntity> InsertAsync(TEntity data, RepoOptions? options = null)
        {
            var sqlQuery = GetInsertQuery(data);
            var jsonData = sqlQuery.Data.GetJson();
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            var (connection, closeConnection) = OpenConnection(options);
            try
            {
                var rows = await connection.QueryAsync<Int64>(sqlQuery.Sql, sqlQuery.Data, options?.Transaction);
                if (rows.Any())
                {
                    Int64 id = rows.First();
                    SetId(data, id);
                }

                return data;
            }
            finally
            {
                closeConnection();
            }
        }

        public async Task<int> GetCountAsync(QueryOptions options)
        {
            var sqlQueryParts = GetSelectQueryParts(options);
            var query = "SELECT COUNT(*) " + sqlQueryParts.SqlFrom;
            var jsonData = sqlQueryParts.Data.GetJson();
            Logger.LogDebug("{query}\n{jsonData}", query, jsonData);
            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                return await connection.QuerySingleOrDefaultAsync<int>(
                    query,
                    sqlQueryParts.Data,
                    options.RepoOptions?.Transaction
                );
            }
            finally
            {
                closeConnection();
            }
        }

        public async Task<TEntity?> GetSingleOrDefaultAsync(QueryOptions options)
        {
            options = new QueryOptions(options) { Top = 2 };
            var list = await GetListAsync(options);
            var count = list.Count();
            if (count == 0)
                return null;

            if (count > 1)
                throw new TooManyRowsException();

            return list.First();
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(QueryOptions options)
        {
            options = new QueryOptions(options) { Top = 1 };
            var list = await GetListAsync(options);
            var count = list.Count();
            if (count == 0)
                return null;

            return list.First();
        }

        public async Task<TEntity> GetSingleAsync(QueryOptions options)
        {
            options = new QueryOptions(options) { Top = 2 };
            var list = await GetListAsync(options);
            var count = list.Count();
            if (count == 0)
                throw new NoRowsException();

            if (count > 1)
                throw new TooManyRowsException();

            return list.First();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(QueryOptions options)
        {
            var joins = options.Join.Where(i => !string.IsNullOrWhiteSpace(i.PropertyName))
                .ToList();
            if (joins.Count == 0)
            {
                var sqlQuery = GetSelectQuery(options);
                var jsonData = sqlQuery.Data.GetJson();
                Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
                var (connection, closeConnection) = OpenConnection(options.RepoOptions);
                try
                {
                    return await connection.QueryAsync<TEntity>(
                        sqlQuery.Sql,
                        sqlQuery.Data,
                        options.RepoOptions?.Transaction
                    );
                }
                catch (Exception)
                {
                    closeConnection();
                    throw;
                }
                finally
                {
                    closeConnection();
                }
            }

            if (joins.Count == 1)
            {
                var join = joins.First();
                var type = typeof(TEntity);
                var pIncluded1 = type.GetProperty(join.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join.PropertyName ?? "<NULL>"} does not exist");

                var getListAsyncMethod = this.GetType().GetMethod("GetListWith1IncludesAsync")
                    ?? throw new Exception("Error to get GetListWith1IncludesAsync method");

                var genericGetListAsyncMethod = getListAsyncMethod.MakeGenericMethod(pIncluded1.PropertyType);

                var task = (Task?)genericGetListAsyncMethod.Invoke(
                    this,
                    [options, joins]
                )
                    ?? throw new Exception("No result for query");

                var resultProperty = task.GetType().GetProperty("Result");
                return (IEnumerable<TEntity>?)resultProperty?.GetValue(task)
                    ?? throw new Exception("No result for query");
            }

            if (joins.Count == 2) 
            {
                var type = typeof(TEntity);
                var join1 = joins.First();
                var pIncluded1 = type.GetProperty(join1.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join1.PropertyName ?? "<NULL>"} does not exist");
                var join2 = joins.ElementAt(1);
                var pIncluded2 = type.GetProperty(join2.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join2.PropertyName ?? "<NULL>"} does not exist");

                var getListAsyncMethod = this.GetType().GetMethod("GetListWith2IncludesAsync")
                    ?? throw new Exception("Error to get GetListWith2IncludesAsync method");

                var genericGetListAsyncMethod = getListAsyncMethod.MakeGenericMethod(pIncluded1.PropertyType, pIncluded2.PropertyType);

                var task = (Task?)genericGetListAsyncMethod.Invoke(
                    this,
                    [options, joins]
                )
                    ?? throw new Exception("No result for query");

                var resultProperty = task.GetType().GetProperty("Result");
                return (IEnumerable<TEntity>?)resultProperty?.GetValue(task)
                    ?? throw new Exception("No result for query");
            }

            if (joins.Count == 3)
            {
                var type = typeof(TEntity);
                var join1 = joins.First();
                var pIncluded1 = type.GetProperty(join1.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join1.PropertyName ?? "<NULL>"} does not exist");
                var join2 = joins.ElementAt(1);
                var pIncluded2 = type.GetProperty(join2.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join2.PropertyName ?? "<NULL>"} does not exist");
                var join3 = joins.ElementAt(2);
                var pIncluded3 = type.GetProperty(join3.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join3.PropertyName ?? "<NULL>"} does not exist");

                var getListAsyncMethod = this.GetType().GetMethod("GetListWith3IncludesAsync")
                    ?? throw new Exception("Error to get GetListWith3IncludesAsync method");

                var genericGetListAsyncMethod = getListAsyncMethod.MakeGenericMethod(pIncluded1.PropertyType, pIncluded2.PropertyType, pIncluded3.PropertyType);

                var task = (Task?)genericGetListAsyncMethod.Invoke(
                    this,
                    [options, joins]
                )
                    ?? throw new Exception("No result for query");

                var resultProperty = task.GetType().GetProperty("Result");
                return (IEnumerable<TEntity>?)resultProperty?.GetValue(task)
                    ?? throw new Exception("No result for query");
            }

            if (joins.Count == 4)
            {
                var type = typeof(TEntity);
                var join1 = joins.First();
                var pIncluded1 = type.GetProperty(join1.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join1.PropertyName ?? "<NULL>"} does not exist");
                var join2 = joins.ElementAt(1);
                var pIncluded2 = type.GetProperty(join2.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join2.PropertyName ?? "<NULL>"} does not exist");
                var join3 = joins.ElementAt(2);
                var pIncluded3 = type.GetProperty(join3.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join3.PropertyName ?? "<NULL>"} does not exist");
                var join4 = joins.ElementAt(3);
                var pIncluded4 = type.GetProperty(join4.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join4.PropertyName ?? "<NULL>"} does not exist");

                var getListAsyncMethod = this.GetType().GetMethod("GetListWith4IncludesAsync")
                    ?? throw new Exception("Error to get GetListWith4IncludesAsync method");

                var genericGetListAsyncMethod = getListAsyncMethod.MakeGenericMethod(
                    pIncluded1.PropertyType,
                    pIncluded2.PropertyType,
                    pIncluded3.PropertyType,
                    pIncluded4.PropertyType
                );

                var task = (Task?)genericGetListAsyncMethod.Invoke(
                    this,
                    [options, joins]
                )
                    ?? throw new Exception("No result for query");

                var resultProperty = task.GetType().GetProperty("Result");
                return (IEnumerable<TEntity>?)resultProperty?.GetValue(task)
                    ?? throw new Exception("No result for query");
            }
            
            if (joins.Count == 5)
            {
                var type = typeof(TEntity);
                var join1 = joins.First();
                var pIncluded1 = type.GetProperty(join1.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join1.PropertyName ?? "<NULL>"} does not exist");
                var join2 = joins.ElementAt(1);
                var pIncluded2 = type.GetProperty(join2.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join2.PropertyName ?? "<NULL>"} does not exist");
                var join3 = joins.ElementAt(2);
                var pIncluded3 = type.GetProperty(join3.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join3.PropertyName ?? "<NULL>"} does not exist");
                var join4 = joins.ElementAt(3);
                var pIncluded4 = type.GetProperty(join4.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join4.PropertyName ?? "<NULL>"} does not exist");
                var join5 = joins.ElementAt(4);
                var pIncluded5 = type.GetProperty(join5.PropertyName ?? "")
                    ?? throw new Exception($"Error property {join5.PropertyName ?? "<NULL>"} does not exist");

                var getListAsyncMethod = this.GetType().GetMethod("GetListWith5IncludesAsync")
                    ?? throw new Exception("Error to get GetListWith5IncludesAsync method");

                var genericGetListAsyncMethod = getListAsyncMethod.MakeGenericMethod(
                    pIncluded1.PropertyType,
                    pIncluded2.PropertyType,
                    pIncluded3.PropertyType,
                    pIncluded4.PropertyType,
                    pIncluded5.PropertyType
                );

                var task = (Task?)genericGetListAsyncMethod.Invoke(
                    this,
                    [options, joins]
                )
                    ?? throw new Exception("No result for query");

                var resultProperty = task.GetType().GetProperty("Result");
                return (IEnumerable<TEntity>?)resultProperty?.GetValue(task)
                    ?? throw new Exception("No result for query");
            }

            throw new TooManyJoinsException();
        }

        public async Task<IEnumerable<TEntity>> GetListWith1IncludesAsync<TIncluded1>(QueryOptions options, List<From> joins)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = sqlQuery.Data.GetJson();
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);

            var type = typeof(TEntity);
            var join = joins.First();
            var pIncluded1 = type.GetProperty(join.PropertyName ?? "")
                ?? throw new Exception($"Error property {join.PropertyName ?? "<NULL>"} does not exist");

            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                return await connection.QueryAsync<TEntity, TIncluded1, TEntity>(
                    sqlQuery.Sql,
                    (row, included) =>
                    {
                        if (IsValidObject(included))
                            pIncluded1.SetValue(row, included);

                        return row;
                    },
                    sqlQuery.Data,
                    splitOn: options.Separator,
                    transaction: options.RepoOptions?.Transaction
                );
            }
            finally
            {
                closeConnection();
            }
        }

        public async Task<IEnumerable<TEntity>> GetListWith2IncludesAsync<TIncluded1, TIncluded2>(QueryOptions options, List<From> joins)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = sqlQuery.Data.GetJson();
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);

            var type = typeof(TEntity);
            var join1 = joins.First();
            var pIncluded1 = type.GetProperty(join1.PropertyName ?? "")
                ?? throw new Exception($"Error property {join1.PropertyName ?? "<NULL>"} does not exist");
            var join2 = joins.ElementAt(1);
             var pIncluded2 = type.GetProperty(join2.PropertyName ?? "")
                ?? throw new Exception($"Error property {join2.PropertyName ?? "<NULL>"} does not exist");

            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                return await connection.QueryAsync<TEntity, TIncluded1, TIncluded2, TEntity>(
                    sqlQuery.Sql,
                    (row, included1, included2) =>
                    {
                        if (IsValidObject(included1))
                            pIncluded1.SetValue(row, included1);

                        if (IsValidObject(included2))
                            pIncluded2.SetValue(row, included2);

                        return row;
                    },
                    sqlQuery.Data,
                    splitOn: options.Separator,
                    transaction: options.RepoOptions?.Transaction
                );
            }
            finally
            {
                closeConnection();
            }
        }

        public async Task<IEnumerable<TEntity>> GetListWith3IncludesAsync<TIncluded1, TIncluded2, TIncluded3>(QueryOptions options, List<From> joins)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = sqlQuery.Data.GetJson();
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);

            var type = typeof(TEntity);
            var join1 = joins.First();
            var pIncluded1 = type.GetProperty(join1.PropertyName ?? "")
                ?? throw new Exception($"Error property {join1.PropertyName ?? "<NULL>"} does not exist");
            var join2 = joins.ElementAt(1);
            var pIncluded2 = type.GetProperty(join2.PropertyName ?? "")
               ?? throw new Exception($"Error property {join2.PropertyName ?? "<NULL>"} does not exist");
            var join3 = joins.ElementAt(2);
            var pIncluded3 = type.GetProperty(join3.PropertyName ?? "")
               ?? throw new Exception($"Error property {join3.PropertyName ?? "<NULL>"} does not exist");

            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                return await connection.QueryAsync<TEntity, TIncluded1, TIncluded2, TIncluded3, TEntity>(
                    sqlQuery.Sql,
                    (row, included1, included2, included3) =>
                    {
                        if (IsValidObject(included1))
                            pIncluded1.SetValue(row, included1);

                        if (IsValidObject(included2))
                            pIncluded2.SetValue(row, included2);

                        if (IsValidObject(included3))
                            pIncluded3.SetValue(row, included3);

                        return row;
                    },
                    sqlQuery.Data,
                    splitOn: options.Separator,
                    transaction: options.RepoOptions?.Transaction
                );
            }
            finally
            {
                closeConnection();
            }
        }

        public async Task<IEnumerable<TEntity>> GetListWith4IncludesAsync<TIncluded1, TIncluded2, TIncluded3, TIncluded4>(QueryOptions options, List<From> joins)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = sqlQuery.Data.GetJson();
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);

            var type = typeof(TEntity);
            var join1 = joins.First();
            var pIncluded1 = type.GetProperty(join1.PropertyName ?? "")
                ?? throw new Exception($"Error property {join1.PropertyName ?? "<NULL>"} does not exist");
            var join2 = joins.ElementAt(1);
            var pIncluded2 = type.GetProperty(join2.PropertyName ?? "")
               ?? throw new Exception($"Error property {join2.PropertyName ?? "<NULL>"} does not exist");
            var join3 = joins.ElementAt(2);
            var pIncluded3 = type.GetProperty(join3.PropertyName ?? "")
               ?? throw new Exception($"Error property {join3.PropertyName ?? "<NULL>"} does not exist");
            var join4 = joins.ElementAt(3);
            var pIncluded4 = type.GetProperty(join4.PropertyName ?? "")
               ?? throw new Exception($"Error property {join4.PropertyName ?? "<NULL>"} does not exist");

            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                return await connection.QueryAsync<TEntity, TIncluded1, TIncluded2, TIncluded3, TIncluded4, TEntity>(
                    sqlQuery.Sql,
                    (row, included1, included2, included3, included4) =>
                    {
                        if (IsValidObject(included1))
                            pIncluded1.SetValue(row, included1);

                        if (IsValidObject(included2))
                            pIncluded2.SetValue(row, included2);

                        if (IsValidObject(included3))
                            pIncluded3.SetValue(row, included3);

                        if (IsValidObject(included4))
                            pIncluded4.SetValue(row, included4);

                        return row;
                    },
                    sqlQuery.Data,
                    splitOn: options.Separator,
                    transaction: options.RepoOptions?.Transaction
                );
            }
            finally
            {
                closeConnection();
            }
        }

        public async Task<IEnumerable<TEntity>> GetListWith5IncludesAsync<TIncluded1, TIncluded2, TIncluded3, TIncluded4, TIncluded5>(QueryOptions options, List<From> joins)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = sqlQuery.Data.GetJson();
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);

            var type = typeof(TEntity);
            var join1 = joins.First();
            var pIncluded1 = type.GetProperty(join1.PropertyName ?? "")
                ?? throw new Exception($"Error property {join1.PropertyName ?? "<NULL>"} does not exist");
            var join2 = joins.ElementAt(1);
            var pIncluded2 = type.GetProperty(join2.PropertyName ?? "")
               ?? throw new Exception($"Error property {join2.PropertyName ?? "<NULL>"} does not exist");
            var join3 = joins.ElementAt(2);
            var pIncluded3 = type.GetProperty(join3.PropertyName ?? "")
               ?? throw new Exception($"Error property {join3.PropertyName ?? "<NULL>"} does not exist");
            var join4 = joins.ElementAt(3);
            var pIncluded4 = type.GetProperty(join4.PropertyName ?? "")
               ?? throw new Exception($"Error property {join4.PropertyName ?? "<NULL>"} does not exist");
            var join5 = joins.ElementAt(4);
            var pIncluded5 = type.GetProperty(join5.PropertyName ?? "")
               ?? throw new Exception($"Error property {join5.PropertyName ?? "<NULL>"} does not exist");

            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                return await connection.QueryAsync<TEntity, TIncluded1, TIncluded2, TIncluded3, TIncluded4, TIncluded5, TEntity>(
                    sqlQuery.Sql,
                    (row, included1, included2, included3, included4, included5) =>
                    {
                        if (IsValidObject(included1))
                            pIncluded1.SetValue(row, included1);

                        if (IsValidObject(included2))
                            pIncluded2.SetValue(row, included2);

                        if (IsValidObject(included3))
                            pIncluded3.SetValue(row, included3);

                        if (IsValidObject(included4))
                            pIncluded4.SetValue(row, included4);

                        if (IsValidObject(included5))
                            pIncluded5.SetValue(row, included5);

                        return row;
                    },
                    sqlQuery.Data,
                    splitOn: options.Separator,
                    transaction: options.RepoOptions?.Transaction
                );
            }
            finally
            {
                closeConnection();
            }
        }

        public async Task<int> UpdateAsync(IDataDictionary data, QueryOptions options)
        {
            var sqlQuery = GetUpdateQuery(data, options);
            var jsonData = sqlQuery.Data.GetJson();
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                return await connection.ExecuteAsync(
                    sqlQuery.Sql,
                    sqlQuery.Data,
                    options.RepoOptions?.Transaction
                );
            }
            catch (Exception)
            {
                Console.WriteLine(sqlQuery.Sql);
                closeConnection();
                throw;
            }
            finally
            {
                closeConnection();
            }
        }

        public async Task<int> DeleteAsync(QueryOptions options)
        {
            var sqlQuery = GetDeleteQuery(options);
            var jsonData = sqlQuery.Data.GetJson();
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                return await connection.ExecuteAsync(
                    sqlQuery.Sql,
                    sqlQuery.Data,
                    options.RepoOptions?.Transaction
                );
            }
            finally
            {
                closeConnection();
            }
        }

        public async Task<Int64?> GetInt64Async(QueryOptions options)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = sqlQuery.Data.GetJson();
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                var rows = await connection.QueryAsync<Int64?>(
                    sqlQuery.Sql,
                    sqlQuery.Data,
                    options?.RepoOptions.Transaction
                );
                if (rows.Any())
                    return rows.First();
            }
            finally
            {
                closeConnection();
            }

            throw new NoRowsException();
        }
    }
}
