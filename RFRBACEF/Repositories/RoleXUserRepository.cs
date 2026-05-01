using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;
using RFRBACIRepositories.IRepositories;

namespace RFRBACEF.Repositories
{
    public class RoleXUserRepository
        : CommonJoinRepository<RoleXUser>,
        IRoleXUserRepository
    {
        public RoleXUserRepository(DbContext context) : base(context) { }

        public override IQueryable<RoleXUser> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options ?? new BaseQueryOptions());

            if (options is RoleXUserQueryOptions roleXUserOptions)
            {
                if (roleXUserOptions.IncludeRole)
                {
                    queryable = queryable.Include(r => r.Role);
                }

                if (roleXUserOptions.IncludeUser)
                {
                    queryable = queryable.Include(u => u.User);
                }
            }

            return queryable;
        }

        public async Task<IEnumerable<long>> GetListRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        {
            var set = CreateDBSet(options);
            var roleIds = await set
                .Where(x =>  x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync();
            return roleIds;
        }
    }
}
