using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;
using RFRBACIRepositories.IRepositories;

namespace RFRBACEF.Repositories
{
    public class PermissionXRoleRepository
        : CommonJoinRepository<PermissionXRole>,
        IPermissionXRoleRepository
    {
        public PermissionXRoleRepository(DbContext context) : base(context) { }

        public override IQueryable<PermissionXRole> CreateDBSet(BaseQueryOptions? options)
        {
            var queryable = base.CreateDBSet(options ?? new BaseQueryOptions());

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

        public async Task<IEnumerable<long>> GetAllPermissionsIdByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null)
        {
            var set = CreateDBSet(options);
            var result = await set
                .Where(r => roleIds.Contains(r.RoleId))
                .Select(r => r.PermissionId)
                .ToListAsync();

            return result;
        }
    }
}
