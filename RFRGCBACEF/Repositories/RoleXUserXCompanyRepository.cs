using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRGCBACEntities.Entities;
using RFRGCBACEntities.QueryOptions;
using RFRGCBACIRepositories.IRepositories;

namespace RFRGCBACEF.Repositories
{
    public class RoleXUserXCompanyRepository
        : CommonJoinRepository<RoleXUserXCompany>,
        IRoleXUserXCompanyRepository
    {
        public RoleXUserXCompanyRepository(DbContext context) : base(context) { }

        public override IQueryable<RoleXUserXCompany> CreateDBSet(BaseQueryOptions? options = null)
        {
            var quereable = base.CreateDBSet(options ?? new BaseQueryOptions())
                as IQueryable<RoleXUserXCompany>
                ?? throw new Exception("Error creating RoleXUserXCompanyRepository");

            if (options is RoleXUserXCompanyQueryOptions roleXUserXCompanyOptions)
            {
                if (roleXUserXCompanyOptions.IncludeRole)
                {
                    quereable = quereable.Include(r => r.Role);
                }

                if (roleXUserXCompanyOptions.IncludeUser)
                {
                    quereable = quereable.Include(u => u.User);
                }

                if (roleXUserXCompanyOptions.IncludeCompany)
                {
                    quereable = quereable.Include(c => c.Company);
                }
            }

            return quereable;
        }

        public async Task<IEnumerable<long>> GetListIdByUserIdAndCompanyIdAsync(long userId, long? companyId, RoleXUserXCompanyQueryOptions? options = null)
        {
            var set = CreateDBSet(options);

            var list = await set
                .Where(e => e.UserId == userId && (companyId == null || e.CompanyId == companyId))
                .Select(e => e.RoleId)
                .ToListAsync();

            return list;
        }

        public async Task<IEnumerable<Company>> GetListCompaniesByUserIdAsync(long userId, RoleXUserXCompanyQueryOptions? options = null)
        {
            var set = CreateDBSet(options);

            var list = await set
                .Where(e => e.UserId == userId)
                .Select(e => e.Company)
                .Distinct()
                .Where(c => c != null)
                .Select(c => c!)
                .ToListAsync();

            return list;
        }
    }
}
