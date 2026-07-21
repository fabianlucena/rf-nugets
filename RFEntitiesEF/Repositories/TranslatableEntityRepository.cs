using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories
{
    public class TranslatableEntityRepository<T>(DbContext context)
        : CommonEntityRepository<T>(context)
        where T : TranslatableEntity, new()
    {
        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            return queryable;
        }
    }
}
