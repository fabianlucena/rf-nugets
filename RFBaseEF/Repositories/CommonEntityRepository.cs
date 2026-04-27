using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class CommonEntityRepository<T>
        : AuditableEntityRepository<T>
        where T : CommonEntity, new()
    {
        public CommonEntityRepository(DbContext context) : base(context) { }

        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options)
        {
            var quereable = base.CreateDBSet(options ?? new BaseQueryOptions());

            if (options is CommonEntityQueryOptions commonOptions)
            {
                if (!commonOptions.IncludeDeleted)
                {
                    quereable = quereable.Where(u => u.DeletedAt == null);
                }

                if (commonOptions.IncludeDeletedBy)
                {
                    quereable = quereable.Include(u => u.DeletedBy);
                }
            }

            return quereable;
        }
    }
}
