using RFBase.ILibs;
using RFBase.Libs;
using RFEntities.Entities;
using RFQueryBuilder.Interfaces;
using RFQueryBuilder.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFQueryBuilder.Implementations;

public class QueryBuilder<T> : IQueryBuilder<T>
    where T : Base, new()
{
    private Table? _table;
    private Table Table
    {
        get => _table ?? throw new InvalidOperationException("Table is not set.");

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
                throw new InvalidOperationException("Table name is not set.");

            return tableName;
        }
    }

    private List<Column> _columns = [];
    public List<Column> Columns
    {
        get
        {
            if (_columns.Count <= 0)
            {
                var type = typeof(T);
                var properties = type.GetProperties();
                _columns = properties.Select(p =>
                {
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
                        Name = SanitizeColumnName(column),
                        Alias = SanitizeColumnAlias(alias),
                    };
                }).ToList() ?? [];
            }

            return _columns;
        }
    }
    public List<string> ColumnsAlias
    {
        get => [..Columns.Select(c => c.Name + " AS " + c.Alias)];
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

    public virtual string SanitizeColumnAlias(string alias)
        => alias;

    public string BuildSelectQuery()
    {
        var selectClause = "SELECT";
        var distinctClause = _distinct ? "DISTINCT" : "";
        var columnsClause = string.Join(", ", ColumnsAlias);
        var fromClause = $"FROM {TableName}";
        var whereClause = _where.Length > 0 ? $"WHERE {string.Join(" AND ", _where)}" : "";
        var orderByClause = _orderBy.Length > 0 ? $"ORDER BY {string.Join(", ", _orderBy)}" : "";
        var limitClause = _take > 0 ? $"LIMIT {_take}" : "";
        var offsetClause = _skip > 0 ? $"OFFSET {_skip}" : "";
        return $"{selectClause} {distinctClause} {columnsClause} {fromClause} {whereClause} {orderByClause} {limitClause} {offsetClause}".Trim();
    }
}
