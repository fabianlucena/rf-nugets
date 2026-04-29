using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;
using RFRGOBACIRepositories.IRepositories;

namespace RFRGOBACEF.Repositories
{
    public class RoleXUserXOrganizationRepository
        : CommonJoinRepository<RoleXUserXOrganization>,
        IRoleXUserXOrganizationRepository
    {
        public RoleXUserXOrganizationRepository(DbContext context) : base(context) { }

        public override IQueryable<RoleXUserXOrganization> CreateDBSet(BaseQueryOptions? options = null)
        {
            var quereable = base.CreateDBSet(options ?? new BaseQueryOptions())
                as IQueryable<RoleXUserXOrganization>
                ?? throw new Exception("Error creating RoleXUserXOrganizationRepository");

            if (options is RoleXUserXOrganizationQueryOptions roleXUserXOrganizationOptions)
            {
                if (roleXUserXOrganizationOptions.IncludeRole)
                {
                    quereable = quereable.Include(r => r.Role);
                }

                if (roleXUserXOrganizationOptions.IncludeUser)
                {
                    quereable = quereable.Include(u => u.User);
                }

                if (roleXUserXOrganizationOptions.IncludeOrganization)
                {
                    quereable = quereable.Include(c => c.Organization);
                }
            }

            return quereable;
        }

        public async Task<IEnumerable<long>> GetListIdByUserIdAndOrganizationIdAsync(long userId, long? OrganizationId, RoleXUserXOrganizationQueryOptions? options = null)
        {
            var set = CreateDBSet(options);

            var list = await set
                .Where(e => e.UserId == userId && (OrganizationId == null || e.OrganizationId == OrganizationId))
                .Select(e => e.RoleId)
                .ToListAsync();

            return list;
        }

        public async Task<IEnumerable<Organization>> GetListCompaniesByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null)
        {
            var set = CreateDBSet(options);

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
