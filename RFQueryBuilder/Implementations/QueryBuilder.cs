using RFBase.ILibs;
using RFBase.Libs;
using RFEntities.Attributes;
using RFEntities.Entities;
using RFQueryBuilder.Exceptions;
using RFQueryBuilder.Interfaces;
using RFQueryBuilder.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFQueryBuilder.Implementations;

public class QueryBuilder<T> : IQueryBuilder<T>
    where T : Base, new()
{
    static readonly Dictionary<Type, Dictionary<Type, QueryBuilder<T>>> cache = [];

    private EntityTable EntityTable { get; }
    
    public List<EntityColumn> EntityColumns { get; }
    public List<EntityColumn> PrimaryKeyColumns { get; }
    public List<EntityColumn> SelectableColumns { get; }
    public List<EntityColumn> InsertableColumns { get; }
    public List<EntityColumn> UpdatableColumns { get; }

    private readonly List<Column> AllColumns;
    private List<Column> _selectedColumns = [];
    public List<Column> SelectedColumns
    {
        get
        {
            if (_selectedColumns.Count <= 0)
                return AllColumns;

            return _selectedColumns;
        }
    }

    public List<string> SelectedColumnsAlias
    {
        get => [.. SelectedColumns.Select(c => c.Query + " AS " + c.Alias)];
    }

    private bool _distinct = false;
    private string[] _where = [];
    private string[] _orderBy = [];
    private int _take = 0;
    private int _skip = 0;

    public DataDictionary Params { get; private set; } = [];

    public QueryBuilder()
    {
        var qbType = this.GetType();
        var type = typeof(T);
        if (cache.TryGetValue(qbType, out var qbData) && qbData.TryGetValue(typeof(T), out var cachedInstance))
        {
            EntityTable = cachedInstance.EntityTable;
            EntityColumns = cachedInstance.EntityColumns;
            PrimaryKeyColumns = cachedInstance.PrimaryKeyColumns;
            SelectableColumns = cachedInstance.SelectableColumns;
            InsertableColumns = cachedInstance.InsertableColumns;
            UpdatableColumns = cachedInstance.UpdatableColumns;
            AllColumns = cachedInstance.AllColumns;
            return;
        }

        var tableName = type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() is TableAttribute tableAttribute ?
            tableAttribute.Name :
            type.Name;

        EntityTable = new EntityTable
        {
            Name = tableName,
            Query = SanitizeTableName(tableName),
        };

        PrimaryKeyColumns = [];
        var properties = type.GetProperties();
        EntityColumns = properties.Select(p =>
        {
            var isMapeable = !(p.GetGetMethod()?.IsVirtual ?? false)
                && IsSimpleType(p.PropertyType)
                && p.GetCustomAttributes(typeof(VirtualAttribute), true).Length == 0
                && p.GetCustomAttributes(typeof(NotMappedAttribute), true).FirstOrDefault() is not NotMappedAttribute;

            var isPrimaryKey = p.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault() is KeyAttribute;

            string alias = p.Name, column;
            if (p.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() is ColumnAttribute columnAttribute
                && !string.IsNullOrWhiteSpace(columnAttribute.Name)
            )
            {
                column = columnAttribute.Name;
            }
            else
            {
                column = alias;
            }

            var entityColumn = new EntityColumn(
                column,
                p.PropertyType,
                SanitizeColumnName(column),
                SanitizeColumnAlias(alias),
                isMapeable,
                isPrimaryKey
            );

            if (isPrimaryKey)
                PrimaryKeyColumns.Add(entityColumn);

            return entityColumn;
        }).Where(c => c != null).Select(c => c!).ToList() ?? [];

        var hasAutoincrementPrimaryKey = PrimaryKeyColumns.Any(c => c.IsPrimaryKey && (c.Type == typeof(int) || c.Type == typeof(long)));

        SelectableColumns = [.. EntityColumns.Where(c => c.IsMapeable)];
        AllColumns = [.. SelectableColumns.Select(c => new Column(c.Query, c.Alias))];
        if (hasAutoincrementPrimaryKey)
        {
            InsertableColumns = [.. EntityColumns.Where(c => c.IsMapeable && !c.IsPrimaryKey)];
        }
        else
        {
            InsertableColumns = [.. EntityColumns.Where(c => c.IsMapeable)];
        }

        UpdatableColumns = [.. EntityColumns.Where(c => c.IsMapeable && !c.IsPrimaryKey)];

        if (qbData is null)
        {
            qbData = [];
            cache[qbType] = qbData;

            qbData[type] = this.Clone();
        }
    }

    protected virtual QueryBuilder<T> Clone()
    {
        var clone = (QueryBuilder<T>)MemberwiseClone();
        clone.Params = [];
        clone._selectedColumns = [];
        clone._where = [];
        clone._orderBy = [];
        clone._take = 0;
        clone._skip = 0;
        return clone;
    }

    private static bool IsSimpleType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        return type.IsPrimitive
            || type.IsEnum
            || type == typeof(string)
            || type == typeof(decimal)
            || type == typeof(DateTime)
            || type == typeof(Guid)
            || type == typeof(TimeSpan);
    }

    public IQueryBuilder<T> AddParam(string key, object? value)
    {
        Params[key] = value;
        return this;
    }

    public IQueryBuilder<T> AddParams(object? param)
    {
        Params.AddFrom(param);
        return this;
    }

    public IQueryBuilder<T> Distinct(bool distinct = true)
    {
        _distinct = distinct;
        return this;
    }

    public IQueryBuilder<T> Select(params string[] columns)
    {
        foreach (var column in columns)
        {
            if (!_selectedColumns.Any(c => c.Query == column))
                continue;

            var col = EntityColumns.FirstOrDefault(c => c.Name == column)
                ?? throw new ColumnDoesNotExistInTableException(column, EntityTable.Name);

            _selectedColumns.Add(col);
        }

        return this;
    }

    public IQueryBuilder<T> OrderBy(string orderBy, object? param = null)
    {
        _orderBy = [.. _orderBy, orderBy];
        AddParams(param);
        return this;
    }

    public IQueryBuilder<T> Where(string where, object? param = null)
    {
        _where = [.. _where, where];
        AddParams(param);
        return this;
    }

    public IQueryBuilder<T> WhereColumn(string column, object? value)
    {
        var columnInfo = EntityColumns.Find(c => c.Name == column && c.IsMapeable)
            ?? throw new ColumnDoesNotExistInTableException(column, EntityTable.Name);

        Where($"{columnInfo.Query} = @{columnInfo.Name}");
        AddParam(columnInfo.Name, SanitizeValue(value));

        return this;
    }

    public IQueryBuilder<T> Where(T entity)
    {
        foreach (var column in SelectableColumns)
        {
            var property = typeof(T).GetProperty(column.Name)
                ?? throw new NoEntityPropertyFoundForColumnException(column.Name, EntityTable.Name);

            var value = property.GetValue(entity);

            if (value is null)
            {
                Where($"{column.Query} IS NULL");
            }
            else
            {
                Where($"{column.Query} = @{column.Name}");
                AddParam(column.Name, SanitizeValue(value));
            }
        }

        return this;
    }

    public IQueryBuilder<T> WhereInserted(T entity)
    {
        foreach (var column in InsertableColumns)
        {
            var property = typeof(T).GetProperty(column.Name)
                ?? throw new NoEntityPropertyFoundForColumnException(column.Name, EntityTable.Name);

            var value = property.GetValue(entity);

            if (value is null)
            {
                Where($"{column.Query} IS NULL");
            }
            else
            {
                Where($"{column.Query} = @{column.Name}");
                AddParam(column.Name, SanitizeValue(value));
            }
        }

        return this;
    }

    public IQueryBuilder<T> Take(int take)
    {
        _take = take;
        return this;
    }

    public IQueryBuilder<T> Skip(int skip)
    {
        _skip = skip;
        return this;
    }

    public virtual string SanitizeTableName(string table)
        => table;

    public virtual string SanitizeColumnName(string column)
        => column;

    public virtual string RawColumnName(string column)
        => column;

    public virtual string SanitizeColumnAlias(string alias)
        => alias;

    public virtual object? SanitizeValue(object? value, Column? column = null)
        => value;

    public string BuildSelectQuery()
    {
        var columns = SelectedColumnsAlias;
        if (columns.Count <= 0)
            throw new NoColumnsSelectedException(EntityTable.Name);

        var selectClause = "SELECT";
        var distinctClause = _distinct ? "DISTINCT" : "";
        var columnsClause = string.Join(", ", columns);
        var fromClause = $"FROM {EntityTable.Query}";
        var whereClause = _where.Length > 0 ? $"WHERE {string.Join(" AND ", _where)}" : "";
        var orderByClause = _orderBy.Length > 0 ? $"ORDER BY {string.Join(", ", _orderBy)}" : "";
        var limitClause = _take > 0 ? $"LIMIT {_take}" : "";
        var offsetClause = _skip > 0 ? $"OFFSET {_skip}" : "";
        return $"{selectClause} {distinctClause} {columnsClause} {fromClause} {whereClause} {orderByClause} {limitClause} {offsetClause}".Trim();
    }

    public string BuildSelectCountQuery()
    {
        var selectClause = "SELECT COUNT(*)";
        var fromClause = $"FROM {EntityTable.Query}";
        var whereClause = _where.Length > 0 ? $"WHERE {string.Join(" AND ", _where)}" : "";
        return $"{selectClause} {fromClause} {whereClause}".Trim();
    }

    public virtual string BuildInsertQuery(T entity)
    {
        var columns = new List<EntityColumn>();
        var type = typeof(T);
        foreach (var column in InsertableColumns)
        {
            var property = type.GetProperty(column.Name);
            if (property == null)
                continue;

            columns.Add(column);
            var value = property.GetValue(entity);
            AddParam(column.Name, SanitizeValue(value));
        }

        var insertClause = $"INSERT INTO {EntityTable.Query} ({string.Join(", ", columns.Select(c => c.Query))})";
        var valuesClause = $"VALUES ({string.Join(", ", columns.Select(c => "@" + c.Name))})";

        return $"{insertClause} {valuesClause}".Trim();
    }

    public string BuildUpdateQuery(IDataDictionary data)
    {
        var setClauses = new List<string>();
        foreach (var item in data)
        {
            var column = UpdatableColumns.Find(c => c.Name == item.Key);
            if (column is null)
            {
                if (EntityColumns.Any(c => c.Name == item.Key))
                    throw new ColumnIsNotUpdatableException(item.Key, EntityTable.Name);

                throw new ColumnDoesNotExistInTableException(item.Key, EntityTable.Name);
            }

            AddParam(column.Name, SanitizeValue(item.Value));
            setClauses.Add($"{column.Query} = @{column.Name}");
        }
        var updateClause = $"UPDATE {EntityTable.Query}";
        var setClause = $"SET {string.Join(", ", setClauses)}";
        var whereClause = _where.Length > 0 ? $"WHERE {string.Join(" AND ", _where)}" : "";

        return $"{updateClause} {setClause} {whereClause}".Trim();
    }

    public string BuildDeleteQuery()
    {
        var deleteClause = $"DELETE FROM {EntityTable.Query}";
        var whereClause = _where.Length > 0 ? $"WHERE {string.Join(" AND ", _where)}" : "";
        return $"{deleteClause} {whereClause}".Trim();
    }
}
