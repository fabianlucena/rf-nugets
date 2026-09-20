using RFBase.ILibs;
using RFDapper.Exceptions;
using RFDapper.Interfaces;
using RFEntities.Entities;
using RFIServices.QueryOptions;
using System.Data;
using Dapper;
using RFQueryBuilder.Interfaces;
using RFQueryBuilder.Implementations;

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

    public virtual IQueryBuilder<T> CreateQueryBuilder<D>(BaseQueryOptions? options)
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

    public virtual IQueryBuilder<T> GetQueryBuilder(BaseQueryOptions? options)
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

    public Task<T> CreateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(BaseQueryOptions options)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetCountAsync(BaseQueryOptions options)
    {
        throw new NotImplementedException();
    }

    async public Task<IEnumerable<T>> GetListAsync(BaseQueryOptions options)
    {
        var db = await CreateConnectionAsync();
        var queryBuilder = GetQueryBuilder(options);
        var query = queryBuilder.BuildSelectQuery();
        return await db.QueryAsync<T>(query, queryBuilder.Params);
    }

    public Task<int> UpdateAsync(IDataDictionary data, BaseQueryOptions options)
    {
        throw new NotImplementedException();
    }
}
