using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories;

public class NominableEntityRepository<T>(DbContext context)
    : CommonEntityRepository<T>(context)
    where T : NominableEntity, new()
{
    public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        if (options is NominableEntityQueryOptions nominableQueryOptions)
        {
            if (nominableQueryOptions.Name is not null)
                queryable = queryable.Where(e => e.Name == nominableQueryOptions.Name);

            if (nominableQueryOptions.Names is not null)
                queryable = queryable.Where(e => nominableQueryOptions.Names.Contains(e.Name));
        }

        return queryable;
    }

    public async Task<IEnumerable<string>> GetNamesAsync(NominableEntityQueryOptions options)
    {
        var list = await GetDBSet(options)
            .Select(e => e.Name)
            .ToListAsync();

        return list;
    }
}
