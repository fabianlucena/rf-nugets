using RFBase.ILibs;
using RFBase.Libs;
using RFDapper.Interfaces;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFDapper.Services;

public class QueryBuilder<T> : IQueryBuilder<T>
    where T : Base, new()
{
    private string _table = string.Empty;
    private List<List<string>> _columns = [];
    private bool _distinct = false;
    private string[] _where = [];
    private string[] _orderBy = [];
    private int _take = 0;
    private int _skip = 0;

    public DataDictionary Params { get; } = [];

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

    public virtual string SanitizeTable(string table)
        => table;

    public virtual string SanitizeColumn(string column)
        => column;

    public virtual string SanitizeColumnAlias(string alias)
        => alias;

    public string GetTable()
    {
        if (string.IsNullOrWhiteSpace(_table))
            return _table;

        var type = typeof(T);
        if (type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() is TableAttribute tableAttribute)
        {
            _table = SanitizeTable(tableAttribute.Name);
        }
        else
        {
            _table = type.Name;
        }

        return _table;
    }

    public List<string> GetColumnsAlias()
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

                return new List<string> { SanitizeColumn(column), SanitizeColumnAlias(alias) };
            }).ToList() ?? [["*"]];
        }

        return [.. _columns.Select(c => c[0] + " AS " + c[1])];
    }

    public string BuildSelectQuery()
    {
        var tableName = GetTable();
        var selectClause = "SELECT";
        var distinctClause = _distinct ? "DISTINCT" : "";
        var columnsClause = string.Join(", ", GetColumnsAlias());
        var fromClause = $"FROM {tableName}";
        var whereClause = _where.Length > 0 ? $"WHERE {string.Join(" AND ", _where)}" : "";
        var orderByClause = _orderBy.Length > 0 ? $"ORDER BY {string.Join(", ", _orderBy)}" : "";
        var limitClause = _take > 0 ? $"LIMIT {_take}" : "";
        var offsetClause = _skip > 0 ? $"OFFSET {_skip}" : "";
        return $"{selectClause} {distinctClause} {columnsClause} {fromClause} {whereClause} {orderByClause} {limitClause} {offsetClause}".Trim();
    }
}
