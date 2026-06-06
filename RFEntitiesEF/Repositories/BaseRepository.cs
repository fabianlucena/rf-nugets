using Microsoft.EntityFrameworkCore;
using RFBase.Libs;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories
{
    public class BaseRepository<T>(DbContext context)
        where T : Base, new()
    {
        public virtual IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            IQueryable<T> queryable = context.Set<T>()
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
            var set = context.Set<T>();
            set.Add(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(BaseQueryOptions? options = null)
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
                context.Attach(entity);

                foreach (var kv in data)
                {
                    context.Entry(entity).Property(kv.Key).CurrentValue = kv.Value;
                    context.Entry(entity).Property(kv.Key).IsModified = true;
                }

                await context.SaveChangesAsync();
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
}
