using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;
using RFRGOBACIRepositories.IRepositories;

namespace RFRGOBACEF.Repositories
{
    public class SessionCompanyRepository
        : NoIdEntityRepository<SessionCompany>,
        ISessionCompanyRepository
    {
        public SessionCompanyRepository(DbContext context) : base(context) { }

        public override IQueryable<SessionCompany> CreateDBSet(BaseQueryOptions? options = null)
        {
            var quereable = base.CreateDBSet(options ?? new BaseQueryOptions())
                as IQueryable<SessionCompany>
                ?? throw new Exception("Error creating SessionCompanyRepository");

            if (options is SessionCompanyQueryOptions sessionCompanyOptions)
            {
                if (sessionCompanyOptions.IncludeSession)
                {
                    quereable = quereable.Include(sc => sc.Session);
                }

                if (sessionCompanyOptions.IncludeCompany)
                {
                    quereable = quereable.Include(sc => sc.Company);
                }
            }

            return quereable;
        }

        public async Task<Company?> GetSingleOrDefaultCompanyBySessionIdAsync(long sessionId, SessionCompanyQueryOptions? options = null)
        {
            var set = CreateDBSet(options);
            var company = await set
                .Where(e => e.SessionId == sessionId)
                .Select(e => e.Company)
                .FirstOrDefaultAsync();

            return company;
        }
        
        public async Task<Company> GetSingleCompanyBySessionIdAsync(long sessionId, SessionCompanyQueryOptions? options = null)
        {
            var company = await GetSingleOrDefaultCompanyBySessionIdAsync(sessionId, options);
            if (company == null)
            {
                throw new Exception($"Company with SessionId {sessionId} not found.");
            }

            return company;
        }
    }
}
