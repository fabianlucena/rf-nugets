using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories
{
    public class CommonJoinRepository<T>(DbContext context)
        : CreatableJoinRepository<T>(context)
        where T : CommonJoin, new()
    {
        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is CommonEntityQueryOptions softDeletableOptions)
            {
                if (!softDeletableOptions.IncludeDeleted)
                {
                    queryable = queryable.Where(u => u.DeletedAt == null);
                }

                if (softDeletableOptions.IncludeDeletedBy)
                {
                    queryable = queryable.Include(u => u.DeletedBy);
                }
            }

            return queryable;
        }
    }
}
