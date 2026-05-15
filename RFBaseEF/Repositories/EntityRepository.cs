using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.ILibs;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class EntityRepository<T>
        : BaseRepository<T>
        where T : Entity, new()
    {
        public EntityRepository(DbContext context) : base(context) { }

        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is EntityQueryOptions entityOptions)
            {
                if (!entityOptions.SkipOrderById)
                    queryable = queryable.OrderBy(x => x.Id);

                if (entityOptions.Id != null)
                    queryable = queryable.Where(e => e.Id == entityOptions.Id);

                if (entityOptions.Uuid != null)
                    queryable = queryable.Where(e => e.Uuid == entityOptions.Uuid);
            }

            return queryable;
        }

        public virtual async Task<IEnumerable<long>> GetIdsAsync(EntityQueryOptions? options = null)
        {
            var list = await GetDBSet(options)
                .Select(e => e.Id)
                .ToListAsync();

            return list;
        }

        public virtual async Task<int> UpdateByIdAsync(long id, IDataDictionary data)
        {
            var entity = new T { Id = id };
            context.Set<T>().Attach(entity);

            foreach (var item in data)
            {
                context.Entry(entity).Property(item.Key).CurrentValue = item.Value;
                context.Entry(entity).Property(item.Key).IsModified = true;
            }

            var result = await context.SaveChangesAsync();
            context.Entry(entity).State = EntityState.Detached;

            return result;
        }
    }
}
