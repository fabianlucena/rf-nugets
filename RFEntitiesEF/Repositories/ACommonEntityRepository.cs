using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories
{
    public class ACommonEntityRepository<T>(DbContext context)
        : CommonEntityRepository<T>(context)
        where T : ACommonEntity, new()
    {
        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            var includeInactive = false;

            if (options is ACommonEntityQueryOptions aCommonOptions)
            {
                if (aCommonOptions.IncludeInactive)
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
}
