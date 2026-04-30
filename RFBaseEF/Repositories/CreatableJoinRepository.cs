using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class CreatableJoinRepository<T>
        : JoinRepository<T>
        where T : CreatableJoin, new()
    {
        public CreatableJoinRepository(DbContext context) : base(context) { }

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
