using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories
{
    public class AuditableEntityRepository<T>
        : CreatableEntityRepository<T>
        where T : AuditableEntity, new()
    {
        public AuditableEntityRepository(DbContext context) : base(context) { }

        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

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
