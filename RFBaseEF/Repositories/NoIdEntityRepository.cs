using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class NoIdEntityRepository<T>(DbContext context)
        : BaseRepository<T>(context)
        where T : NoIdEntity, new()
    {
        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is NoIdEntityQueryOptions noIdOptions)
            {
                if (noIdOptions.IncludeCreatedBy)
                {
                    queryable = queryable.Include(u => u.CreatedBy);
                }

                if (noIdOptions.IncludeUpdatedBy)
                {
                    queryable = queryable.Include(u => u.UpdatedBy);
                }

                if (!noIdOptions.IncludeDeleted)
                {
                    queryable = queryable.Where(u => u.DeletedAt == null);
                }

                if (noIdOptions.IncludeDeletedBy)
                {
                    queryable = queryable.Include(u => u.DeletedBy);
                }
            }

            return queryable;
        }
    }
}
