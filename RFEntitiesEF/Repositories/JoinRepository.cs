using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories
{
    public class JoinRepository<T>(DbContext context)
        : BaseRepository<T>(context)
        where T : Join, new()
    {
        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            return queryable;
        }
    }
}
