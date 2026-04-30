using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;
using RFRBACIRepositories.IRepositories;

namespace RFRBACEF.Repositories
{
    public class RoleIncludeRepository
        : CommonJoinRepository<RoleInclude>,
        IRoleIncludeRepository
    {
        public RoleIncludeRepository(DbContext context) : base(context) { }

        public override IQueryable<RoleInclude> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options ?? new BaseQueryOptions());

            if (options is RoleIncludeQueryOptions roleIncludeOptions)
            {
                if (roleIncludeOptions.IncludeRole)
                {
                    queryable = queryable.Include(r => r.Role);
                }

                if (roleIncludeOptions.IncludeInclude)
                {
                    queryable = queryable.Include(r => r.Include);
                }
            }

            return queryable;
        }

        public async Task<IEnumerable<long>> GetAllRolesIdByRolesIdAsync(IEnumerable<long> rolesId, RoleIncludeQueryOptions? options = null)
        {
            var set = CreateDBSet(options);
            var result = rolesId.ToList();
            var lastResult = rolesId.ToList();

            do
            {
                lastResult = await set
                    .Where(r => lastResult.Contains(r.RoleId)
                        && !result.Contains(r.IncludeId)
                    )
                    .Select(r => r.IncludeId)
                    .ToListAsync();

                result.AddRange(lastResult);
            } while (lastResult.Count != 0);

            return result;
        }
    }
}
