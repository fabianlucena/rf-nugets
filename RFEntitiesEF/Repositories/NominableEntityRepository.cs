using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories
{
    public class NominableEntityRepository<T>(DbContext context)
        : CommonEntityRepository<T>(context)
        where T : NominableEntity, new()
    {
        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is NominableEntityQueryOptions nominableQueryOptions)
            {
                if (!string.IsNullOrEmpty(nominableQueryOptions.Name))
                    queryable = queryable.Where(e => e.Name == nominableQueryOptions.Name);
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
}
