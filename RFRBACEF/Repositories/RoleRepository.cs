using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;
using RFRBACIRepositories.IRepositories;

namespace RFRBACEF.Repositories
{
    public class RoleRepository(DbContext context)
        : CreatableEntityRepository<Role>(context),
        IRoleRepository
    {
        public override IQueryable<Role> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is RoleQueryOptions roleOptions)
            {
                if (roleOptions.Ids is not null)
                    queryable = queryable.Where(p => roleOptions.Ids.Contains(p.Id));
            }

            return queryable;
        }

        public async Task<IEnumerable<string>> GetNamesAsync(RoleQueryOptions options)
        {
            return await GetDBSet(options)
                .Select(r => r.Name)
                .ToListAsync();
        }
    }
}
