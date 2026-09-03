using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories;

public class TitledEntityRepository<T>(DbContext context)
    : NominableEntityRepository<T>(context)
    where T : TitledEntity, new()
{
    public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        return queryable;
    }
}
