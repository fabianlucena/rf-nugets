using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.QueryOptions;

namespace RFRBACEF.Repositories
{
    public class RoleRepository(DbContext context)
        : LocalizableEntityRepository<Role>(context),
        IRoleRepository
    {
        public override IQueryable<Role> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is RoleQueryOptions roleOptions)
            {
                if (roleOptions.Ids is not null)
                    queryable = queryable.Where(r => roleOptions.Ids.Contains(r.Id));
            }

            return queryable;
        }
    }
}
