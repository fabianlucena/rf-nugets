using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class BaseRepository<T>
        where T : Base, new()
    {
        public DbContext context;

        public BaseRepository(DbContext _context)
        {
            context = _context;
        }

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
    }
}
