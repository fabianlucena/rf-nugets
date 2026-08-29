using Microsoft.EntityFrameworkCore;
using RFBase.Libs;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories;

public class BaseRepository<T>(DbContext context)
    where T : Base, new()
{
    public DbContext Context { get; } = context;

    public virtual IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
    {
        IQueryable<T> queryable = Context.Set<T>()
            .AsNoTracking();

        if (options != null)
        {
            if (options.Distinct)
                queryable = queryable.Distinct();
        }

        return queryable;
    }

    public virtual IQueryable<T> GetDBSet(BaseQueryOptions? options = null)
    {
        var queryable = CreateDBSet(options);
        if (options != null)
        {
            queryable = queryable
                .Take(options.Take)
                .Skip(options.Skip);
        }

        return queryable;
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        var set = Context.Set<T>();
        set.Add(entity);
        await Context.SaveChangesAsync();
        Context.Entry(entity).State = EntityState.Detached;

        return entity;
    }

    public virtual async Task<IEnumerable<T>> GetListAsync(BaseQueryOptions options)
    {
        var list = await GetDBSet(options)
            .ToListAsync();

        return list;
    }

    public virtual async Task<int> UpdateAsync(DataDictionary data, BaseQueryOptions options)
    {
        var list = await GetListAsync(options);

        var result = 0;
        foreach (var entity in list)
        {
            Context.Attach(entity);

            foreach (var kv in data)
            {
                Context.Entry(entity).Property(kv.Key).CurrentValue = kv.Value;
                Context.Entry(entity).Property(kv.Key).IsModified = true;
            }

            await Context.SaveChangesAsync();
            result++;
        }

        return result;
    }

    public virtual async Task<int> DeleteAsync(BaseQueryOptions options)
    {
        var result = await GetDBSet()
            .ExecuteDeleteAsync();

        return result;
    }
}
