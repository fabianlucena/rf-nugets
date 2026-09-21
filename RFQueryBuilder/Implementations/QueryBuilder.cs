using RFBase.ILibs;
using RFBase.Libs;
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
    private Table? _table;
    private Table Table
    {
        get => _table ?? throw new TableIsNotSetException();

        set
        {
            _table = value;
            if (_table.Entity is not null)
            {
                var type = _table.Entity;
                if (type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() is TableAttribute tableAttribute)
                {
                    _table.Name = SanitizeTableName(tableAttribute.Name);
                }
                else
                {
                    _table.Name = SanitizeTableName(type.Name);
                }
            }
        }
    }
    public string TableName
    {
        get
        {
            var tableName = Table.Name;
            if (string.IsNullOrWhiteSpace(tableName))
                throw new TableNameIsNotSetException();

            return tableName;
        }
    }

    private List<Column> _tableColumns = [];
    public List<Column> TableColumns
    {
        get
        {
            if (_tableColumns.Count <= 0)
            {
                var type = typeof(T);
                var properties = type.GetProperties();
                _tableColumns = properties.Select(p =>
                {
                    if (!IsSimpleType(p.PropertyType))
                        return null;

                    if (p.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault() is KeyAttribute)
                        return null;

                    if (p.GetCustomAttributes(typeof(NotMappedAttribute), true).FirstOrDefault() is NotMappedAttribute)
                        return null;

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

                    return new Column
                    {
                        Name = column,
                        Query = SanitizeColumnName(column),
                        Alias = SanitizeColumnAlias(alias),
                    };
                }).Where(c => c != null).Select(c => c!).ToList() ?? [];
            }

            return _tableColumns;
        }
    }
    public List<string> TableColumnsAlias
    {
        get => [..TableColumns.Select(c => c.Query + " AS " + c.Alias)];
    }

    private readonly List<Column> _selectedColumns = [];
    public List<Column> SelectedColumns
    {
        get
        {
            if (_selectedColumns.Count <= 0)
                return TableColumns;

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

    public DataDictionary Params { get; } = [];

    public QueryBuilder()
    {
        Table = new Table { Entity = typeof(T) };
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
            if (!_selectedColumns.Any(c => c.Name == column))
                continue;

            var col = TableColumns.FirstOrDefault(c => c.Name == column)
                ?? throw new ColumnDoesNotExistInTableException(column, TableName);

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
        var columnInfo = TableColumns.Find(c => c.Name == column)
            ?? throw new ColumnDoesNotExistInTableException(column, TableName);

        Where($"{columnInfo.Query} = @{columnInfo.Name}");
        AddParam(columnInfo.Name, SanitizeValue(value));

        return this;
    }

    public IQueryBuilder<T> Where(T entity)
    {
        foreach (var column in TableColumns)
        {
            var property = typeof(T).GetProperty(column.Name);
            if (property == null)
                continue;

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
        var selectClause = "SELECT";
        var distinctClause = _distinct ? "DISTINCT" : "";
        var columnsClause = string.Join(", ", SelectedColumnsAlias);
        var fromClause = $"FROM {TableName}";
        var whereClause = _where.Length > 0 ? $"WHERE {string.Join(" AND ", _where)}" : "";
        var orderByClause = _orderBy.Length > 0 ? $"ORDER BY {string.Join(", ", _orderBy)}" : "";
        var limitClause = _take > 0 ? $"LIMIT {_take}" : "";
        var offsetClause = _skip > 0 ? $"OFFSET {_skip}" : "";
        return $"{selectClause} {distinctClause} {columnsClause} {fromClause} {whereClause} {orderByClause} {limitClause} {offsetClause}".Trim();
    }

    public string BuildInsertQuery(T entity)
    {
        var columns = new List<Column>();
        var type = typeof(T);
        foreach (var column in TableColumns)
        {
            var property = type.GetProperty(column.Name);
            if (property == null)
                continue;

            columns.Add(column);
            var value = property.GetValue(entity);
            AddParam(column.Name, SanitizeValue(value));
        }

        var insertClause = $"INSERT INTO {TableName} ({string.Join(", ", columns.Select(c => c.Query))})";
        var valuesClause = $"VALUES ({string.Join(", ", columns.Select(c => "@" + c.Name))})";

        return $"{insertClause} {valuesClause}".Trim();
    }

    public string BuildUpdateQuery(IDataDictionary data)
    {
        var setClauses = new List<string>();
        foreach (var item in data)
        {
            var column = TableColumns.Find(c => c.Name == item.Key)
                ?? throw new ColumnDoesNotExistInTableException(item.Key, TableName);

            AddParam(column.Name, SanitizeValue(item.Value));
            setClauses.Add($"{column.Query} = @{column.Name}");
        }
        var updateClause = $"UPDATE {TableName}";
        var setClause = $"SET {string.Join(", ", setClauses)}";
        var whereClause = _where.Length > 0 ? $"WHERE {string.Join(" AND ", _where)}" : "";

        return $"{updateClause} {setClause} {whereClause}".Trim();
    }
}
