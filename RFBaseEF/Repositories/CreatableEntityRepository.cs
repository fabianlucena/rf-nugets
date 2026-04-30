using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class CreatableEntityRepository<T>
        : EntityRepository<T>
        where T : CreatableEntity, new()
    {
        public CreatableEntityRepository(DbContext context) : base(context) { }

        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is CreatableEntityQueryOptions creatableOptions)
            {
                if (creatableOptions.IncludeCreatedBy)
                {
                    queryable = queryable.Include(u => u.CreatedBy);
                }
            }

            return queryable;
        }
    }
}
