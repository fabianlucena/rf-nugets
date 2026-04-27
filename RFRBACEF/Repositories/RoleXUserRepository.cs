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
            var quereable = base.CreateDBSet(options ?? new BaseQueryOptions());

            if (options is RoleXUserQueryOptions roleXUserOptions)
            {
                if (roleXUserOptions.IncludeRole)
                {
                    quereable = quereable.Include(r => r.Role);
                }

                if (roleXUserOptions.IncludeUser)
                {
                    quereable = quereable.Include(u => u.User);
                }
            }

            return quereable;
        }

        public async Task<IEnumerable<long>> GetListRolesIdByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        {
            var set = CreateDBSet(options);
            var rolesId = await set
                .Where(x =>  x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync();
            return rolesId;
        }
    }
}
