using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories;

public class ALocalizableEntityRepository<T>(DbContext context)
    : LocalizableEntityRepository<T>(context)
    where T : ALocalizableEntity, new()
{
    public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        var includeInactive = false;

        if (options is ALocalizableEntityQueryOptions aLocalizableOptions)
        {
            if (aLocalizableOptions.IncludeInactive)
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
