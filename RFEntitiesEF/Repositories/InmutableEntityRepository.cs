using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories
{
    public class ImmutableEntityRepository<T>(DbContext context)
        : BaseRepository<T>(context)
        where T : ImmutableEntity, new()
    {
        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is CommonEntityQueryOptions commonOptions)
            {
                if (!commonOptions.IncludeDeleted)
                {
                    queryable = queryable.Where(u => u.DeletedAt == null);
                }

                if (commonOptions.IncludeDeletedBy)
                {
                    queryable = queryable.Include(u => u.DeletedBy);
                }
            }

            return queryable;
        }
    }
}
