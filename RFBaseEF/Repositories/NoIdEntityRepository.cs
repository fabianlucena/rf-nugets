using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class NoIdEntityRepository<T>
        : BaseRepository<T>
        where T : NoIdEntity, new()
    {
        public NoIdEntityRepository(DbContext context) : base(context) { }

        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options)
        {
            var quereable = base.CreateDBSet(options ?? new BaseQueryOptions());

            if (options is NoIdEntityQueryOptions noIdOptions)
            {
                if (noIdOptions.IncludeCreatedBy)
                {
                    quereable = quereable.Include(u => u.CreatedBy);
                }

                if (noIdOptions.IncludeUpdatedBy)
                {
                    quereable = quereable.Include(u => u.UpdatedBy);
                }

                if (!noIdOptions.IncludeDeleted)
                {
                    quereable = quereable.Where(u => u.DeletedAt == null);
                }

                if (noIdOptions.IncludeDeletedBy)
                {
                    quereable = quereable.Include(u => u.DeletedBy);
                }
            }

            return quereable;
        }
    }
}
