using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class JoinRepository<T>
        : BaseRepository<T>
        where T : Join, new()
    {
        public JoinRepository(DbContext context) : base(context) { }

        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var quereable = base.CreateDBSet(options);

            return quereable;
        }
    }
}
