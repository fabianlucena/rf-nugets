using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories;

public class CreatableEntityRepository<T>(DbContext context)
    : EntityRepository<T>(context)
    where T : CreatableEntity, new()
{
    public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        if (options is CreatableEntityQueryOptions creatableOptions)
        {
            if (creatableOptions.IncludeCreatedBy)
                queryable = queryable.Include(u => u.CreatedBy);

            if (creatableOptions.CreatedAfter is not null)
                queryable = queryable.Where(e => e.CreatedAt >= creatableOptions.CreatedAfter);
        }

        return queryable;
    }
}
