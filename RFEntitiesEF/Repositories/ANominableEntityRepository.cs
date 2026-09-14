using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories;

public class ANominableEntityRepository<T>(DbContext context)
    : NominableEntityRepository<T>(context)
    where T : ANominableEntity, new()
{
    public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        var includeInactive = false;

        if (options is ANominableEntityQueryOptions aNominableOptions)
        {
            if (aNominableOptions.IncludeInactive)
            {
                includeInactive = true;
            }
        }

        if (!includeInactive)
        {
            queryable = queryable.Where(u => u.IsActive);
        }

        return queryable;
    }
}
