using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;
using RFRBACIRepositories.IRepositories;

namespace RFRBACEF.Repositories
{
    public class PermissionXRoleRepository(DbContext context)
        : CommonJoinRepository<PermissionXRole>(context),
        IPermissionXRoleRepository
    {
        public override IQueryable<PermissionXRole> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is PermissionXRoleQueryOptions sessionOptions)
            {
                if (sessionOptions.IncludePermission)
                {
                    queryable = queryable.Include(p => p.Permission);
                }

                if (sessionOptions.IncludeRole)
                {
                    queryable = queryable.Include(r => r.Role);
                }
            }

            return queryable;
        }

        public async Task<IEnumerable<long>> GetPermissionIdsByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null)
        {
            var set = GetDBSet(options);
            var result = await set
                .Where(r => roleIds.Contains(r.RoleId))
                .Select(r => r.PermissionId)
                .ToListAsync();

            return result;
        }
    }
}
