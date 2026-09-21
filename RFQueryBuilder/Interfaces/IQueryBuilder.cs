using RFBase.Libs;
using RFEntities.Entities;

namespace RFQueryBuilder.Interfaces;

public interface IQueryBuilder<T>
    where T : Base, new()
{
    DataDictionary Params { get; }

    IQueryBuilder<T> Distinct(bool distinct = true);
    IQueryBuilder<T> Select(params string[] columns);
    IQueryBuilder<T> OrderBy(string orderBy, object? param = null);
    IQueryBuilder<T> Where(string where, object? param = null);
    IQueryBuilder<T> WhereColumn(string column, object? value);
    IQueryBuilder<T> Where(T entity);

    IQueryBuilder<T> Take(int take);
    IQueryBuilder<T> Skip(int skip);

    string BuildSelectQuery();
    string BuildInsertQuery(T entity);
}
