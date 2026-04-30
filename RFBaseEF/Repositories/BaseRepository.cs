using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Style",
    "IDE0290:Use primary constructor")]
    public class BaseRepository<T> where T : Base, new()
    {
        protected readonly DbContext context;

        public BaseRepository(DbContext context)
        {
            this.context = context;
        }

        public virtual IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            options ??= new BaseQueryOptions();
            IQueryable<T> queryable = context.Set<T>()
                .AsNoTracking();

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
            options ??= new BaseQueryOptions();

            var set = CreateDBSet(options)
                .Skip(options.Skip)
                .Take(options.Take);

            var list = await set
                .ToListAsync();

            return list;
        }
    }
}
