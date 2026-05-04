using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRGOBACEF.Exceptions;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;
using RFRGOBACIRepositories.IRepositories;

namespace RFRGOBACEF.Repositories
{
    public class RoleXUserXOrganizationRepository(DbContext context)
        : CommonJoinRepository<RoleXUserXOrganization>(context),
        IRoleXUserXOrganizationRepository
    {
        public override IQueryable<RoleXUserXOrganization> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            queryable = queryable.OrderBy(ruo => ruo.UserId);

            if (options is RoleXUserXOrganizationQueryOptions roleXUserXOrganizationOptions)
            {
                if (roleXUserXOrganizationOptions.IncludeRole)
                    queryable = queryable.Include(ruo => ruo.Role);

                if (roleXUserXOrganizationOptions.IncludeUser)
                    queryable = queryable.Include(ruo => ruo.User);

                if (roleXUserXOrganizationOptions.IncludeOrganization)
                    queryable = queryable.Include(ruo => ruo.Organization);
            }

            return queryable;
        }

        public async Task<IEnumerable<long>> GetListIdByUserIdAndOrganizationIdAsync(long userId, long? OrganizationId, RoleXUserXOrganizationQueryOptions? options = null)
        {
            var set = GetDBSet(options);

            var list = await set
                .Where(e => e.UserId == userId && (OrganizationId == null || e.OrganizationId == OrganizationId))
                .Select(e => e.RoleId)
                .ToListAsync();

            return list;
        }

        public async Task<IEnumerable<Organization>> GetListOrganizationsByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null)
        {
            var set = GetDBSet(options);

            var list = await set
                .Where(e => e.UserId == userId)
                .Select(e => e.Organization)
                .Distinct()
                .Where(c => c != null)
                .Select(c => c!)
                .ToListAsync();

            return list;
        }
    }
}
