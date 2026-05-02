using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRGOBACEF.Exceptions;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;
using RFRGOBACIRepositories.IRepositories;

namespace RFRGOBACEF.Repositories
{
    public class SessionOrganizationRepository(DbContext context)
        : NoIdEntityRepository<SessionOrganization>(context),
        ISessionOrganizationRepository
    {
        public override IQueryable<SessionOrganization> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options)
                as IQueryable<SessionOrganization>
                ?? throw new ErrorCreatingSessionOrganizationRepositoryException();

            if (options is SessionOrganizationQueryOptions sessionOrganizationOptions)
            {
                if (sessionOrganizationOptions.IncludeSession)
                {
                    queryable = queryable.Include(sc => sc.Session);
                }

                if (sessionOrganizationOptions.IncludeOrganization)
                {
                    queryable = queryable.Include(sc => sc.Organization);
                }
            }

            return queryable;
        }

        public async Task<Organization?> GetSingleOrDefaultOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null)
        {
            var set = GetDBSet(options);
            var Organization = await set
                .Where(e => e.SessionId == sessionId)
                .Select(e => e.Organization)
                .FirstOrDefaultAsync();

            return Organization;
        }
        
        public async Task<Organization> GetSingleOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null)
        {
            var Organization = (await GetSingleOrDefaultOrganizationBySessionIdAsync(sessionId, options))
                ?? throw new OrganizationWithSessionIdNotFoundException(sessionId);

            return Organization;
        }
    }
}
