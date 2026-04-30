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
            var quereable = base.CreateDBSet(options);

            if (options is CreatableEntityQueryOptions creatableOptions)
            {
                if (creatableOptions.IncludeCreatedBy)
                {
                    quereable = quereable.Include(u => u.CreatedBy);
                }

                quereable = quereable
                    .Skip(options.Skip)
                    .Take(options.Take);
            }

            return quereable;
        }
    }
}
