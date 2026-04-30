using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRGOBACEF.Exceptions;
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
            var queryable = base.CreateDBSet(options ?? new BaseQueryOptions())
                as IQueryable<RoleXUserXOrganization>
                ?? throw new ErrorCreatingRoleXUserXOrganizationRepositoryException();

            if (options is RoleXUserXOrganizationQueryOptions roleXUserXOrganizationOptions)
            {
                if (roleXUserXOrganizationOptions.IncludeRole)
                {
                    queryable = queryable.Include(r => r.Role);
                }

                if (roleXUserXOrganizationOptions.IncludeUser)
                {
                    queryable = queryable.Include(u => u.User);
                }

                if (roleXUserXOrganizationOptions.IncludeOrganization)
                {
                    queryable = queryable.Include(c => c.Organization);
                }
            }

            return queryable;
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

        public async Task<IEnumerable<Organization>> GetListOrganizationsByUserIdAsync(long userId, RoleXUserXOrganizationQueryOptions? options = null)
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
