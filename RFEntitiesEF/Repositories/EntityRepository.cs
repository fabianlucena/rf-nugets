using Microsoft.EntityFrameworkCore;
using RFBase.ILibs;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories
{
    public class EntityRepository<T>(DbContext context)
        : BaseRepository<T>(context)
        where T : Entity, new()
    {
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

        public virtual async Task<int> DeleteByIdAsync(long id)
        {
            var result = await GetDBSet()
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
