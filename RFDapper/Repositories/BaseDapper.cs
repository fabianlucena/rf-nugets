using Dapper;
using RFBase.ILibs;
using RFDapper.Exceptions;
using RFDapper.Extensions;
using RFDapper.Interfaces;
using RFEntities.Entities;
using RFIServices.QueryOptions;
using RFQueryBuilder.Implementations;
using RFQueryBuilder.Interfaces;
using System.Data;

namespace RFDapper.Repositories;

public class BaseDapper<T>
    where T : Base, new()
{
    protected readonly IDbConnection? DbConnection;
    protected readonly IDbConnectionFactory? ConnectionFactory;

    public BaseDapper(IDbConnection dbConnection)
    {
        DbConnection = dbConnection;
    }

    public BaseDapper(IDbConnectionFactory connectionFactory)
    {
        ConnectionFactory = connectionFactory;
    }

    public virtual async Task<IDbConnection> CreateConnectionAsync(CancellationToken ct = default)
    {
        if (DbConnection is not null)
            return DbConnection;

        if (ConnectionFactory is null)
            throw new ConnectionFactoryIsNotInitializedException();

        return await ConnectionFactory.CreateConnectionAsync(ct);
    }

    public virtual IQueryBuilder<T> CreateQueryBuilder<D>(BaseQueryOptions? options = null)
        where D : IQueryBuilder<T>, new()
    {
        var queryBuilder = new D();

        if (options != null)
        {
            if (options.Distinct)
                queryBuilder.Distinct();
        }

        return queryBuilder;
    }

    public virtual IQueryBuilder<T> GetQueryBuilder(BaseQueryOptions? options = null)
    {
        var queryBuilder = CreateQueryBuilder<QueryBuilder<T>>(options);

        if (options != null)
        {
            queryBuilder
                .Take(options.Take)
                .Skip(options.Skip);
        }

        return queryBuilder;
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        var db = await CreateConnectionAsync();
        var insertQB = GetQueryBuilder();
        var insertQuery = insertQB.BuildInsertQuery(entity);
        var parameters = insertQB.Params.ToDynamicParameters();
        await db.ExecuteAsync(insertQuery, parameters);

        var selectQB = GetQueryBuilder();
        selectQB.Where(entity);
        var selectQuery = selectQB.BuildSelectQuery();
        var inserted = db.QuerySingle<T>(selectQuery, selectQB.Params.ToDynamicParameters());
        return inserted;
    }

    public virtual Task<int> DeleteAsync(BaseQueryOptions options)
    {
        throw new NotImplementedException();
    }

    public virtual Task<int> GetCountAsync(BaseQueryOptions options)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<IEnumerable<T>> GetListAsync(BaseQueryOptions options)
    {
        var db = await CreateConnectionAsync();
        var queryBuilder = GetQueryBuilder(options);
        var query = queryBuilder.BuildSelectQuery();
        return await db.QueryAsync<T>(query, queryBuilder.Params.ToDynamicParameters());
    }

    public virtual async Task<int> UpdateAsync(IDataDictionary data, BaseQueryOptions options)
    {
        var db = await CreateConnectionAsync();
        var updateQB = GetQueryBuilder(options);
        var updateQuery = updateQB.BuildUpdateQuery(data);
        return await db.ExecuteAsync(updateQuery, updateQB.Params.ToDynamicParameters());
    }
}
