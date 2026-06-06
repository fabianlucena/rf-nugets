using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.QueryOptions;

namespace RFRBACEF.Repositories
{
    public class RoleIncludeRepository
        : CommonJoinRepository<RoleInclude>,
        IRoleIncludeRepository
    {
        public RoleIncludeRepository(DbContext context) : base(context) { }

        public override IQueryable<RoleInclude> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            queryable = queryable.OrderBy(ri => ri.RoleId);
            
            if (options is RoleIncludeQueryOptions roleIncludeOptions)
            {
                if (roleIncludeOptions.IncludeRole)
                    queryable = queryable.Include(ri => ri.Role);

                if (roleIncludeOptions.IncludeInclude)
                    queryable = queryable.Include(ri => ri.Include);
            }

            return queryable;
        }

        public async Task<IEnumerable<long>> GetAllRoleIdsByRoleIdsAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null)
        {
            var set = GetDBSet(options);
            var result = roleIds.ToList();
            var lastResult = roleIds.ToList();

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
