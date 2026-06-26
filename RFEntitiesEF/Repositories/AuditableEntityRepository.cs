using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories;

public class AuditableEntityRepository<T>(DbContext context)
    : CreatableEntityRepository<T>(context)
    where T : AuditableEntity, new()
{
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
