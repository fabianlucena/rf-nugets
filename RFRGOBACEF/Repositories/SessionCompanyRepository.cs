using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;
using RFRGOBACIRepositories.IRepositories;

namespace RFRGOBACEF.Repositories
{
    public class SessionOrganizationRepository
        : NoIdEntityRepository<SessionOrganization>,
        ISessionOrganizationRepository
    {
        public SessionOrganizationRepository(DbContext context) : base(context) { }

        public override IQueryable<SessionOrganization> CreateDBSet(BaseQueryOptions? options = null)
        {
            var quereable = base.CreateDBSet(options ?? new BaseQueryOptions())
                as IQueryable<SessionOrganization>
                ?? throw new Exception("Error creating SessionOrganizationRepository");

            if (options is SessionOrganizationQueryOptions sessionOrganizationOptions)
            {
                if (sessionOrganizationOptions.IncludeSession)
                {
                    quereable = quereable.Include(sc => sc.Session);
                }

                if (sessionOrganizationOptions.IncludeOrganization)
                {
                    quereable = quereable.Include(sc => sc.Organization);
                }
            }

            return quereable;
        }

        public async Task<Organization?> GetSingleOrDefaultOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null)
        {
            var set = CreateDBSet(options);
            var Organization = await set
                .Where(e => e.SessionId == sessionId)
                .Select(e => e.Organization)
                .FirstOrDefaultAsync();

            return Organization;
        }
        
        public async Task<Organization> GetSingleOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null)
        {
            var Organization = await GetSingleOrDefaultOrganizationBySessionIdAsync(sessionId, options);
            if (Organization == null)
            {
                throw new Exception($"Organization with SessionId {sessionId} not found.");
            }

            return Organization;
        }
    }
}
