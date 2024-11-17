using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace RFDapper
{
    public class SqlQuery
    {
        public string Sql = "";
        public Dictionary<string, object?> Values = [];
    }

    public class Dapper<Entity>: IRepo<Entity>
        where Entity : class
    {
        private readonly ILogger<Dapper<Entity>> _logger;
        private readonly IDbConnection _connection;
        private readonly string _tableName;
        private readonly string _schema = "dbo";

        public ILogger<Dapper<Entity>> Logger { get => _logger; }
        public IDbConnection Connection { get => _connection; }
        public string TableName { get => _tableName; }
        public string Schema { get => _schema; }

        public Dapper(ILogger<Dapper<Entity>> logger, IDbConnection Connection)
        {
            _logger = logger;
            _connection = Connection;
            var tableAttribute = typeof(Entity).GetCustomAttribute<TableAttribute>();
            if (tableAttribute == null)
            {
                _tableName = typeof(Entity).Name;
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

        public void CreateTable()
        {
            var SQLTypes = new Dictionary<string, string>
                {
                    {"Int64", "BIGINT"},
                    {"Guid", "UNIQUEIDENTIFIER"},
                    {"DateTime", "DATETIME"},
                    {"Boolean", "BIT"},
                    {"GeoCoordinate", "GEOGRAPHY"},
                };

            var query = $@"IF NOT EXISTS (SELECT TOP 1 1 FROM sys.schemas WHERE [name] = '{Schema}')
                EXEC('CREATE SCHEMA [{Schema}] AUTHORIZATION [dbo]');";
            Connection.Query(query);

            var entityType = typeof(Entity);
            var properties = entityType.GetProperties();
            var columnsQueries = new List<string>();
            var postQueries = new List<string>();
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                bool? nullable = null;
                string propertyTypeName;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    nullable = true;
                    propertyTypeName = Nullable.GetUnderlyingType(propertyType)?.Name ?? "";
                } else
                {
                    propertyTypeName = propertyType.Name;
                }

                if (!SQLTypes.TryGetValue(propertyTypeName, out string? sqlType))
                {
                    if (!propertyType.IsClass)
                        throw new Exception($"Unknown type {propertyType.Name}");

                    if (propertyTypeName == "String")
                        sqlType = $"NVARCHAR({property.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? 255})";
                }

                if (sqlType != null)
                {
                    var columnQuery = $"[{property.Name}] {sqlType}";

                    var settedPk = false;
                    var databaseGeneratedAttribute = property.GetCustomAttribute<DatabaseGeneratedAttribute>();
                    if (databaseGeneratedAttribute != null)
                    {
                        if (databaseGeneratedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                        {
                            columnQuery += " IDENTITY(1,1)";
                            postQueries.Add($"CONSTRAINT [{Schema}_{TableName}_PK] PRIMARY KEY NONCLUSTERED ({property.Name})");
                            settedPk = true;
                        }
                    }

                    if (!settedPk)
                    {
                        if (property.CustomAttributes.Any(a => a.AttributeType.Name == "RequiredAttribute"))
                            columnQuery += " NOT NULL";
                        else if (Nullable.GetUnderlyingType(propertyType) != null)
                            columnQuery += " NULL";
                        else if (nullable != null)
                        {
                            columnQuery += (bool)nullable ?
                                " NULL" :
                                " NOT NULL";
                        }

                        if (property.GetCustomAttribute<KeyAttribute>() != null)
                            postQueries.Add($"CONSTRAINT [{Schema}_{TableName}_PK_{property.Name}] PRIMARY KEY CLUSTERED ([{property.Name}])");
                    }

                    columnsQueries.Add(columnQuery);
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
                        foreignTable = typeof(Entity).Name;
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
            Connection.Query(query);
        }

        public SqlQuery GetFilterQuery(object? filter, List<string> skipNames, string name = "")
        {
            if (filter == null)
            {
                return new SqlQuery { Sql = " IS NULL" };
            }

            if (filter is Dictionary<string, object?> filters)
            {
                var sqlFilters = new List<string>();
                var values = new Dictionary<string, object?>();
                foreach (var f in filters)
                {
                    var key = f.Key;
                    var value = f.Value;
                    if (value == null)
                    {
                        sqlFilters.Add($"{key} IS NULL");
                        continue;
                    }

                    var newName = name + key;
                    if (skipNames.Contains(name))
                    {
                        var root = newName;
                        int counter = 0;
                        newName = root + counter;
                        while (skipNames.Contains(newName))
                        {
                            counter++;
                        }
                    }
                    skipNames.Add(newName);

                    var sqlQuery = GetFilterQuery(value, skipNames, newName);

                    sqlFilters.Add(key + sqlQuery.Sql);
                    foreach (var kv in sqlQuery.Values)
                        values[kv.Key] = kv.Value;
                }

                return new SqlQuery
                {
                    Sql = string.Join(" AND ", sqlFilters),
                    Values = values,
                };
            }
            
            if (filter is string)
            {
                return new SqlQuery
                {
                    Sql = " = @" + name,
                    Values = { { name, filter } },
                };
            }

            if (filter.GetType().GetInterface("IEnumerable") != null)
            {
                return new SqlQuery
                {
                    Sql = " IN @" + name,
                    Values = { { name, filter } },
                };
            }
            
            if (filter is RFService.Operator.DistinctTo op)
            {
                var sqlQuery = GetFilterQuery(op.Value, skipNames, name);
                var sql = " NOT";
                if (sqlQuery.Sql[0] != ' ')
                    sql += " ";
                sql += sqlQuery.Sql;

                return new SqlQuery
                {
                    Sql = sql,
                    Values = sqlQuery.Values,
                };
            }

            return new SqlQuery
            {
                Sql = " = @" + name,
                Values = { { name, filter } },
            };
        }

        public SqlQuery? GetWhereQuery(GetOptions options, string prefix = "")
        {
            if (options.Filters.Count == 0)
                return null;

            var sqlQuery = GetFilterQuery(options.Filters, [], prefix);
            sqlQuery.Sql = "WHERE " + sqlQuery.Sql;

            return sqlQuery;
        }

        public SqlQuery GetSelectQuery(GetOptions options)
        {
            var sql = $"SELECT * FROM [{Schema}].[{TableName}]";
            Dictionary<string, object?>? values = null;
            if (options != null)
            {
                var where = GetWhereQuery(options);
                if (where != null)
                {
                    sql += " " + where.Sql;
                    values = where.Values;
                }

                if (options.Offset != null)
                    sql += $" OFFSET {options.Offset}";

                if (options.Top != null)
                    sql += $" TOP {options.Top}";
            }

            return new SqlQuery
            {
                Sql = sql,
                Values = values ?? [],
            };
        }

        public SqlQuery GetInsertQuery(Entity data)
        {
            var entityType = typeof(Entity);
            var properties = data.GetType().GetProperties();
            var columns = new List<string>();
            var valuesName = new List<string>();
            Dictionary<string, object?> values = [];

            foreach (var p in properties)
            {
                string name = p.Name;
                if (name == "Id")
                    continue;

                var property = entityType.GetProperty(name) ??
                    throw new Exception($"Unknown {name} property");

                var propertyType = property.PropertyType;
                if (propertyType.IsClass && propertyType.Name != "String")
                    continue;

                var varName = "@" + name;
                columns.Add(name);
                valuesName.Add(varName);
                values[varName] = property.GetValue(data, null);
            }

            var sql = $"INSERT INTO [{Schema}].[{TableName}]({string.Join(",", columns)}) VALUES ({string.Join(",", valuesName)}); SELECT CAST(SCOPE_IDENTITY() as INT);";

            return new SqlQuery
            {
                Sql = sql,
                Values = values,
            };
        }

        public SqlQuery GetUpdateQuery(object data, GetOptions options)
        {
            var sqlWhere = GetWhereQuery(options, "filter_")
                ?? throw new Exception("UPDATE without WHERE is forbidden");

            var dataType = data.GetType();
            var properties = dataType.GetProperties();
            var columns = new List<string>();
            Dictionary<string, object?> values = [];

            foreach (var p in properties)
            {
                string name = p.Name;
                var property = dataType.GetProperty(name) ??
                        throw new Exception($"Unknown {name} property");
                var propertyType = property.PropertyType;

                if (propertyType.IsClass && propertyType.Name != "String")
                    continue;

                var valueName = "data_" + name;
                values[valueName] = p.GetValue(data, null);

                columns.Add($"[{name}]=@{valueName}");
            }

            var sql = $"UPDATE [{Schema}].[{TableName}] SET {string.Join(",", columns)} {sqlWhere}";

            return new SqlQuery
            {
                Sql = sql,
                Values = values,
            };
        }

        static void SetId(Entity data, long id)
        {
            var type = data.GetType();
            var pId = type.GetProperty("Id");
            pId?.SetValue(data, id);
        }

        public async Task<Entity> InsertAsync(Entity data)
        {
            var sqlQuery = GetInsertQuery(data);
            var jsonData = JsonConvert.SerializeObject(sqlQuery.Values);
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            var rows = await Connection.QueryAsync<long>(sqlQuery.Sql, sqlQuery.Values);
            long id = rows.First();
            SetId(data, id);
            return data;
        }

        public Task<Entity?> GetSingleOrDefaultAsync(GetOptions options)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = JsonConvert.SerializeObject(sqlQuery.Values);
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            return Connection.QuerySingleOrDefaultAsync<Entity>(sqlQuery.Sql, sqlQuery.Values);
        }

        public Task<Entity?> GetFirstOrDefaultAsync(GetOptions options)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = JsonConvert.SerializeObject(sqlQuery.Values);
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            return Connection.QueryFirstOrDefaultAsync<Entity>(sqlQuery.Sql, sqlQuery.Values);
        }

        public Task<Entity> GetSingleAsync(GetOptions options)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = JsonConvert.SerializeObject(sqlQuery.Values);
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            return Connection.QuerySingleAsync<Entity>(sqlQuery.Sql, sqlQuery.Values);
        }

        public Task<IEnumerable<Entity>> GetListAsync(GetOptions options)
        {
            var sqlQuery = GetSelectQuery(options);
            var jsonData = JsonConvert.SerializeObject(sqlQuery.Values);
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            return Connection.QueryAsync<Entity>(sqlQuery.Sql, sqlQuery.Values);
        }

        public async Task<int> UpdateAsync(object data, GetOptions options)
        {
            var sqlQuery = GetUpdateQuery(data, options);
            var jsonData = JsonConvert.SerializeObject(sqlQuery.Values);
            Logger.LogDebug("{query}\n{jsonData}", sqlQuery.Sql, jsonData);
            return await Connection.ExecuteAsync(sqlQuery.Sql, sqlQuery.Values);
        }
    }
}
