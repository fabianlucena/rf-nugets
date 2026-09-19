using RFBase.ILibs;
using RFBase.Libs;
using RFDapper.Interfaces;
using RFEntities.Entities;

namespace RFDapper.Services;

public class QueryBuilder<T> : IQueryBuilder<T>
    where T : Base, new()
{
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

    public string BuildSelectQuery()
    {
        var tableName = typeof(T).Name;
        var selectClause = "SELECT";
        var distinctClause = _distinct ? "DISTINCT" : "";
        var columnsClause = "*";
        var fromClause = $"FROM {tableName}";
        var whereClause = _where.Length > 0 ? $"WHERE {string.Join(" AND ", _where)}" : "";
        var orderByClause = _orderBy.Length > 0 ? $"ORDER BY {string.Join(", ", _orderBy)}" : "";
        var limitClause = _take > 0 ? $"LIMIT {_take}" : "";
        var offsetClause = _skip > 0 ? $"OFFSET {_skip}" : "";
        return $"{selectClause} {distinctClause} {columnsClause} {fromClause} {whereClause} {orderByClause} {limitClause} {offsetClause}".Trim();
    }
}
