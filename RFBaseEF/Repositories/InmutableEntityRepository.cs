using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class ImmutableEntityRepository<T>
        : BaseRepository<T>
        where T : ImmutableEntity, new()
    {
        public ImmutableEntityRepository(DbContext context) : base(context)
        {
        }

        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options)
        {
            var queryable = base.CreateDBSet(options ?? new BaseQueryOptions());

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
