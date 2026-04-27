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
            var quereable = base.CreateDBSet(options);

            if (options is CreatableEntityQueryOptions creatableOptions)
            {
                if (creatableOptions.IncludeCreatedBy)
                {
                    quereable = quereable.Include(u => u.CreatedBy);
                }
            }

            return quereable;
        }
    }
}
