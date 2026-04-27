using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class CommonJoinRepository<T>
        : CreatableJoinRepository<T>
        where T : CommonJoin, new()
    {
        public CommonJoinRepository(DbContext context) : base(context) { }

        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options)
        {
            var quereable = base.CreateDBSet(options ?? new BaseQueryOptions());

            if (options is CommonEntityQueryOptions softDeletableOptions)
            {
                if (!softDeletableOptions.IncludeDeleted)
                {
                    quereable = quereable.Where(u => u.DeletedAt == null);
                }

                if (softDeletableOptions.IncludeDeletedBy)
                {
                    quereable = quereable.Include(u => u.DeletedBy);
                }
            }

            return quereable;
        }
    }
}
