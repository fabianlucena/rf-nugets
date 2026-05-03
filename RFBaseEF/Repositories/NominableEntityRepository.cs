using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
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
    }
}
