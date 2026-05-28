using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFPermissionsEntities.Entities;
using RFPermissionsEntities.QueryOptions;
using RFPermissionsIRepositories.Repositories;

namespace RFPermissionsEF.Repositories
{
    public class PermissionRepository(DbContext context)
        : CreatableEntityRepository<Permission>(context),
        IPermissionRepository
    {
        public override IQueryable<Permission> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is PermissionQueryOptions permissionOptions)
            {
                if (permissionOptions.Ids is not null)
                    queryable = queryable.Where(p => permissionOptions.Ids.Contains(p.Id));

                if (permissionOptions.Names is not null)
                    queryable = queryable.Where(p => permissionOptions.Names.Contains(p.Name));
            }

            return queryable;
        }

        public async Task<IEnumerable<string>> GetNamesAsync(PermissionQueryOptions options)
        {
            return await GetDBSet(options)
                .Select(r => r.Name)
                .ToListAsync();
        }
    }
}
