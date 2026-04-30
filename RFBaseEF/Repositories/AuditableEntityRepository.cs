using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class AuditableEntityRepository<T>
        : CreatableEntityRepository<T>
        where T : AuditableEntity, new()
    {
        public AuditableEntityRepository(DbContext context) : base(context) { }

        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options)
        {
            var queryable = base.CreateDBSet(options ?? new BaseQueryOptions());

            if (options is AuditableEntityQueryOptions auditableOptions)
            {
                if (auditableOptions.IncludeUpdatedBy)
                {
                    queryable = queryable.Include(u => u.UpdatedBy);
                }
            }

            return queryable;
        }
    }
}
