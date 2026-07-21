using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories
{
    public class CreatableJoinRepository<T>(DbContext context)
        : JoinRepository<T>(context)
        where T : CreatableJoin, new()
    {
        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is CreatableEntityQueryOptions creatableOptions)
            {
                if (creatableOptions.IncludeCreatedBy)
                {
                    queryable = queryable.Include(u => u.CreatedBy);
                }

                queryable = queryable
                    .Skip(options.Skip)
                    .Take(options.Take);
            }

            return queryable;
        }
    }
}
