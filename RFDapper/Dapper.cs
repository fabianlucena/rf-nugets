using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RFDapper.Exceptions;
using RFOperators;
using RFService.ILibs;
using RFService.IRepo;
using RFService.Libs;
using RFService.Repo;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Reflection;
using System.Text.Json;

namespace RFDapper
{
    public class SqlQueryParts
    {
        public string SqlSelect = "";

        public string SqlFrom = "";

        public Data Data = new();
    }

    public class Dapper<TEntity>
        : IRepo<TEntity>
        where TEntity : class
    {
        private readonly ILogger<Dapper<TEntity>> _logger;
        private readonly IDriver _driver;
        private readonly string _tableName;
        private readonly string _schema = "dbo";

        public ILogger<Dapper<TEntity>> Logger { get => _logger; }
        public string ConnectionString { get => DBConnectionString.Get<TEntity>(); }
        public string TableName { get => _tableName; }
        public string Schema { get => _schema; }

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

            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            return (connection, () => connection.Dispose());
        }

        public void CreateTable()
        {
            var query = $@"IF NOT EXISTS (SELECT TOP 1 1 FROM sys.schemas WHERE [name] = '{Schema}')
                EXEC('CREATE SCHEMA {_driver.GetSchemaName(Schema)} AUTHORIZATION {_driver.GetDefaultSchema()}');";
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            connection.Query(query);

            var entityType = typeof(TEntity);
            var properties = entityType.GetProperties();
            var columnsQueries = new List<string>();
            var postQueries = new List<string>();
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                var columnDefinition = _driver.GetSQLColumnDefinition(property);

                if (columnDefinition != null)
                {
                    columnsQueries.Add(columnDefinition);

                    var settedPk = false;
                    var databaseGeneratedAttribute = property.GetCustomAttribute<DatabaseGeneratedAttribute>();
                    if (databaseGeneratedAttribute != null)
                    {
                        if (databaseGeneratedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                        {
                            postQueries.Add($"CONSTRAINT [{Schema}_{TableName}_PK] PRIMARY KEY NONCLUSTERED ({property.Name})");
                            settedPk = true;
                        }
                    }

                    if (!settedPk && property.GetCustomAttribute<KeyAttribute>() != null)
                        postQueries.Add($"CONSTRAINT [{Schema}_{TableName}_PK_{property.Name}] PRIMARY KEY CLUSTERED ([{property.Name}])");
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
                        if (!propertyType.IsClass)
                            throw new Exception($"Foreign is not and object{foreign.Name}");

                        referenceColumn = foreignObject;
                        foreignObject = property;

                        foreignObjectType = propertyType;
                    }

                    var foreignTableAttribute = foreignObjectType.GetCustomAttribute<TableAttribute>();
                    string foreignTable = "",
                        foreignSchema = "dbo",
                        foreignColumn = "Id";
                    if (foreignTableAttribute == null)
                        foreignTable = typeof(TEntity).Name;
                    else
                    {
                        foreignTable = foreignTableAttribute.Name;
                        if (!string.IsNullOrEmpty(foreignTableAttribute.Schema))
                            foreignSchema = foreignTableAttribute.Schema;
                    }

                    postQueries.Add($"CONSTRAINT [{Schema}_{TableName}_{referenceColumn.Name}_FK_{foreignSchema}_{foreignTable}_{foreignColumn}]"
                        + $" FOREIGN KEY([{referenceColumn.Name}]) REFERENCES [{foreignSchema}].[{foreignTable}]([{foreignColumn}])"
                    );
                }
            }

            var indexes = entityType.GetCustomAttributes<IndexAttribute>();
            if (indexes != null) {
                foreach (var index in indexes)
                {
                    var name = index.Name ?? $"{Schema}_{TableName}_{(index.IsUnique? "U": "I")}K_{string.Join('_', index.PropertyNames)}";
                    var indexType = index.IsUnique ?
                        "UNIQUE" :
                        "INDEX";

                    postQueries.Add($"CONSTRAINT [{name}] {indexType} ([{string.Join("], [", index.PropertyNames)}])");
                }
            }

            var columnsQuery = string.Join(",\r\n\t\t", [.. columnsQueries]);
            if (postQueries.Count > 0)
                columnsQuery += ",\r\n\t\t" + string.Join(",\r\n\t\t", [.. postQueries]);

            query = $"IF NOT EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'{Schema}.{TableName}') AND type in (N'U'))"
                + $"\r\n\tCREATE TABLE [{Schema}].[{TableName}] (\r\n\t\t{columnsQuery}\r\n\t) ON [PRIMARY]";

            _logger.LogDebug("{query}", query);
            connection.Query(query);
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
            GetOptions options,
            List<string> usedNames
        )
        {
            if (op is Column col)
                return new SqlQuery { Sql = _driver.GetColumnName(col.Name, options) };

            if (op is Value val)
            {
                if (val.Data == null)
                    return new SqlQuery { IsNull = true };

                return _driver.GetValue(val.Data, options, usedNames);
            }

            if (op is Unary uop)
            {
                var sqlQuery = GetOperation(uop.Op, options, usedNames);
                sqlQuery.Sql = op switch
                {
                    IsNull =>    $"({sqlQuery.Sql}) IS NULL",
                    IsNotNull => $"({sqlQuery.Sql}) IS NOT NULL",
                    _ => throw new UnknownUnaryOperatorException(op.GetType().Name),
                };
                return sqlQuery;
            }

            if (op is Binary bop)
            {
                var sqlQuery1 = GetOperation(bop.Op1, options, usedNames);
                var sqlQuery2 = GetOperation(bop.Op2, options, usedNames);

                foreach (var kv in sqlQuery2.Data.Values)
                    sqlQuery1.Data.Values[kv.Key] = kv.Value;

                sqlQuery1.Sql = op switch
                {
                    Eq => sqlQuery2.IsNull?
                        $"({sqlQuery1.Sql}) IS NULL":
                        $"({sqlQuery1.Sql}) = ({sqlQuery2.Sql})",
                    NE => sqlQuery2.IsNull ?
                        $"({sqlQuery1.Sql}) IS NOT NULL" :
                        $"({sqlQuery1.Sql}) <> ({sqlQuery2.Sql})",
                    In => $"({sqlQuery1.Sql}) IN {sqlQuery2.Sql}",
                    NotIn => $"({sqlQuery1.Sql}) NOT IN {sqlQuery2.Sql}",
                    ST_Intersects => $"{sqlQuery1.Sql}.STIntersects({sqlQuery2.Sql}) = 1",
                    ST_Contains => $"{sqlQuery1.Sql}.STContains({sqlQuery2.Sql}) = 1",
                    _ => throw new UnknownBinaryOperatorException(op.GetType().Name),
                };
                return sqlQuery1;
            }

            if (op is NAry nop)
            {
                SqlQuery sqlQuery = new();
                List<string> sqls = [];

                foreach (var iop in nop.Ops)
                {
                    var newSqlQuery = GetOperation(iop, options, usedNames);
                    sqls.Add(newSqlQuery.Sql);
                    foreach (var kv in newSqlQuery.Data.Values)
                        sqlQuery.Data.Values[kv.Key] = kv.Value;
                }

                sqlQuery.Sql = op switch
                {
                    And => "(" + string.Join(") AND (", sqls) + ")",
                    Or => "(" + string.Join(") OR (", sqls) + ")",
                    _ => throw new UnknownNAryOperatorException(op.GetType().Name),
                };
                return sqlQuery;
            }

            throw new UnknownOperationException(op.GetType().Name);
        }

        public SqlQuery GetFilterQuery(
            Operators operators,
            GetOptions options,
            List<string> usedNames
        )
        {
            if (operators.Count <= 0)
                return new SqlQuery();

            if (operators.Count == 1)
                return GetOperation(operators[0], options, usedNames);

            return GetOperation(Op.And([.. operators]), options, usedNames);
        }

        public SqlQuery? GetWhereFilter(GetOptions options, List<string>? usedNames = null)
        {
            if (options.Filters.Count == 0)
                return null;

            var sqlQuery = GetFilterQuery(options.Filters, options, usedNames ?? []);

            return sqlQuery;
        }

        public SqlQuery? GetWhereQuery(GetOptions options, List<string>? usedNames = null)
        {
            var filter = GetWhereFilter(options, usedNames);
            if (filter != null)
                filter.Sql = "WHERE " + filter.Sql;

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

        private static string? GetForeignColumnName(string? propertyName)
        {
            var entityType = typeof(TEntity);
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

        public SqlQueryParts GetSelectQueryParts(GetOptions options)
        {
            options = new GetOptions(options);
            options.Alias = options.GetOrCreateAlias("t");

            var sqlSelect = $"SELECT [{options.Alias}].*";
            var sqlFrom = $" FROM [{Schema}].[{TableName}] [{options.Alias}]";
            List<string> usedNames = [];
            SqlQuery? sqlQuery;

            Data data = new();
            if (options != null)
            {
                foreach (var from in options.Join)
                {
                    Type? propertyType = string.IsNullOrEmpty(from.PropertyName)?
                        null:
                        typeof(TEntity)?.GetProperty(from.PropertyName)?.PropertyType;

                    Type entity = from.Entity
                        ?? propertyType
                        ?? throw new NoEntityForJoinException();

                    var joinType = from.Type
                        ?? (IsForeignColumnNullable(from.PropertyName) ? "LEFT OUTER JOIN" : "INNER JOIN");

                    if (string.IsNullOrEmpty(from.Alias))
                        from.Alias = options.CreateAlias("t");

                    Operator? onOperation = from.On;
                    if (onOperation == null)
                    {
                        if (string.IsNullOrWhiteSpace(from.PropertyName))
                            throw new NoOnClauseForJoinWithEntityException();

                        onOperation = Op.Eq(
                            Op.Column($"{from.Alias}.Id"),
                            Op.Column($"{options.Alias}.{Dapper<TEntity>.GetForeignColumnName(from.PropertyName)}")
                        );
                    }

                    sqlQuery = GetOperation(onOperation, options, usedNames);

                    if (!string.IsNullOrWhiteSpace(from.PropertyName))
                        sqlSelect += $", NULL AS {_driver.GetColumnAlias(options.Separator)}, {_driver.GetTableAlias(from.Alias)}.*";

                    sqlFrom += $" {joinType} {GetTableNameForEntity(entity)} {_driver.GetTableAlias(from.Alias)}"
                        + $" ON {sqlQuery.Sql}";

                    foreach (var value in sqlQuery.Data.Values)
                        data.Values.Add(value.Key, value.Value);
                }

                sqlQuery = GetWhereQuery(options, usedNames);
                if (!string.IsNullOrWhiteSpace(sqlQuery?.Sql))
                {
                    sqlFrom += " " + sqlQuery.Sql;
                    foreach (var value in sqlQuery.Data.Values)
                        data.Values.Add(value.Key, value.Value);
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
                    sqlFrom += $" ORDER BY {String.Join(", ", _driver.GetSQLOrderBy(orderBy, options))}";

                if (options.Offset != null || options.Top != null)
                    sqlFrom += $" OFFSET {options.Offset ?? 0} ROWS";

                if (options.Top != null)
                    sqlFrom += $" FETCH NEXT {options.Top} ROWS ONLY";
            }

            return new SqlQueryParts
            {
                SqlSelect = sqlSelect,
                SqlFrom = sqlFrom,
                Data = data,
            };
        }

        public SqlQuery GetSelectQuery(GetOptions options)
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
            var newData = new Data();
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

                var propertyType = property.PropertyType;
                if (propertyType.IsClass
                    && propertyType.Name != "String"
                    && propertyType.Name != "SqlGeography"
                )
                    continue;

                var varName = "@" + name;
                name = $"[{name}]";
                columns.Add(name);
                valuesName.Add(varName);
                newData.Values[varName] = property.GetValue(data, null);
            }

            var sql = $"INSERT INTO [{Schema}].[{TableName}]({string.Join(",", columns)}) VALUES ({string.Join(",", valuesName)});";
            if (hasId)
                sql += " SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

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
            GetOptions options
        )
        {
            options.Alias = "";
            var sqlWhere = GetWhereQuery(options)
                ?? throw new Exception("UPDATE without WHERE is forbidden");

            var entityType = typeof(TEntity);
            var columns = new List<string>();
            DataDictionary values = [];
            List<string> usedNames = [];

            foreach (var item in data)
            {
                var name = item.Key;
                if (entityType.GetProperty(name) == null)
                    continue;

                var value = item.Value;
                if (value != null)
                {
                    JsonElement? valueJsonElement = value as JsonElement?;
                    if (valueJsonElement != null)
                        value = ConvertJsonElement(valueJsonElement.Value);

                    if (value is Operator op) {
                        value = GetOperation(op, options, usedNames);
                    }
                }
                var valueName = "data_" + name;
                values[valueName] = value;
                columns.Add($"[{name}]=@{valueName}");
            }

            if (columns.Count <= 0)
                throw new NothingToUpdateException();

            var sql = $"UPDATE [{Schema}].[{TableName}]"
                + $" SET {string.Join(",", columns)} "
                + sqlWhere.Sql;
            return new SqlQuery
            {
                Sql = sql,
                Data = { Values = values.Concat(sqlWhere.Data.Values).ToDataDictionary() },
            };
        }

        public SqlQuery GetDeleteQuery(GetOptions options)
        {
            var sqlWhere = GetWhereQuery(options)
                ?? throw new Exception("DELETE without WHERE is forbidden");

            DataDictionary values = [];

            var sql = $"DELETE FROM [{Schema}].[{TableName}] {sqlWhere.Sql}";
            return new SqlQuery
            {
                Sql = sql,
                Data = { Values = values.Concat(sqlWhere.Data.Values).ToDataDictionary() },
            };
        }

        static void SetId(TEntity data, long id)
        {
            var type = data.GetType();
            var pId = type.GetProperty("Id");
            pId?.SetValue(data, id);
        }

        public async Task<TEntity> InsertAsync(TEntity data, RepoOptions? options = null)
        {
            var sqlQuery = GetInsertQuery(data);
            var jsonData = JsonConvert.SerializeObject(sqlQuery.Data);
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            var (connection, closeConnection) = OpenConnection(options);
            try
            {
                var rows = await connection.QueryAsync<Int64>(sqlQuery.Sql, sqlQuery.Data.Values, options?.Transaction);
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

        public async Task<int> GetCountAsync(GetOptions options)
        {
            var sqlQueryParts = GetSelectQueryParts(options);
            var query = "SELECT COUNT(*) " + sqlQueryParts.SqlFrom;
            var jsonData = JsonConvert.SerializeObject(sqlQueryParts.Data);
            Logger.LogDebug("{query}\n{jsonData}", query, jsonData);
            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                return await connection.QuerySingleOrDefaultAsync<int>(
                    query,
                    sqlQueryParts.Data.Values,
                    options.RepoOptions?.Transaction
                );
            }
            finally
            {
                closeConnection();
            }
        }


        public async Task<TEntity?> GetSingleOrDefaultAsync(GetOptions options)
        {
            options = new GetOptions(options) { Top = 2 };
            var list = await GetListAsync(options);
            var count = list.Count();
            if (count == 0)
                return null;

            if (count > 1)
                throw new TooManyRowsException();

            return list.First();
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(GetOptions options)
        {
            options = new GetOptions(options) { Top = 1 };
            var list = await GetListAsync(options);
            var count = list.Count();
            if (count == 0)
                return null;

            return list.First();
        }

        public async Task<TEntity> GetSingleAsync(GetOptions options)
        {
            options = new GetOptions(options) { Top = 2 };
            var list = await GetListAsync(options);
            var count = list.Count();
            if (count == 0)
                throw new NoRowsException();

            if (count > 1)
                throw new TooManyRowsException();

            return list.First();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(GetOptions options)
        {
            var joins = options.Join.Where(i => !string.IsNullOrWhiteSpace(i.PropertyName))
                .ToList();
            if (joins.Count == 0)
            {
                var sqlQuery = GetSelectQuery(options);
                var jsonData = JsonConvert.SerializeObject(sqlQuery.Data);
                Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
                var (connection, closeConnection) = OpenConnection(options.RepoOptions);
                try
                {
                    return await connection.QueryAsync<TEntity>(
                        sqlQuery.Sql,
                        sqlQuery.Data.Values,
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

            throw new TooManyJoinsException();
        }

        public async Task<IEnumerable<TEntity>> GetListWith1IncludesAsync<TIncluded1>(GetOptions options, List<From> joins)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = JsonConvert.SerializeObject(sqlQuery.Data);
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
                    (TEntity row, TIncluded1 included) =>
                    {
                        pIncluded1.SetValue(row, included);
                        return row;
                    },
                    sqlQuery.Data.Values,
                    splitOn: options.Separator,
                    transaction: options.RepoOptions?.Transaction
                );
            }
            finally
            {
                closeConnection();
            }
        }

        public async Task<IEnumerable<TEntity>> GetListWith2IncludesAsync<TIncluded1, TIncluded2>(GetOptions options, List<From> joins)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = JsonConvert.SerializeObject(sqlQuery.Data);
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
                    (TEntity row, TIncluded1 included1, TIncluded2 included2) =>
                    {
                        pIncluded1.SetValue(row, included1);
                        pIncluded2.SetValue(row, included2);
                        return row;
                    },
                    sqlQuery.Data.Values,
                    splitOn: options.Separator,
                    transaction: options.RepoOptions?.Transaction
                );
            }
            finally
            {
                closeConnection();
            }
        }

        public async Task<int> UpdateAsync(IDataDictionary data, GetOptions options)
        {
            var sqlQuery = GetUpdateQuery(data, options);
            var jsonData = JsonConvert.SerializeObject(sqlQuery.Data);
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                return await connection.ExecuteAsync(
                    sqlQuery.Sql,
                    sqlQuery.Data.Values,
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

        public async Task<int> DeleteAsync(GetOptions options)
        {
            var sqlQuery = GetDeleteQuery(options);
            var jsonData = JsonConvert.SerializeObject(sqlQuery.Data);
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            var (connection, closeConnection) = OpenConnection(options.RepoOptions);
            try
            {
                return await connection.ExecuteAsync(
                    sqlQuery.Sql,
                    sqlQuery.Data.Values,
                    options.RepoOptions?.Transaction
                );
            }
            finally
            {
                closeConnection();
            }
        }
    }
}
