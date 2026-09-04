using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFRegisterService.Attributes;
using RFRGOBAC.Entities;
using RFRGOBAC.IRepositories;
using RFRGOBAC.QueryOptions;
using RFRGOBACEF.Exceptions;

namespace RFRGOBACEF.Repositories;

[RegisterService]
public class SessionOrganizationRepository(DbContext context)
    : NoIdEntityRepository<SessionOrganization>(context),
    ISessionOrganizationRepository
{
    public override IQueryable<SessionOrganization> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        queryable = queryable.OrderBy(so => so.SessionId);

        if (options is SessionOrganizationQueryOptions sessionOrganizationOptions)
        {
            if (sessionOrganizationOptions.IncludeSession)
                queryable = queryable.Include(so => so.Session);

            if (sessionOrganizationOptions.IncludeOrganization)
                queryable = queryable.Include(so => so.Organization);

            if (sessionOrganizationOptions.SessionId is not null)
                queryable = queryable.Where(so => so.SessionId == sessionOrganizationOptions.SessionId);

            if (sessionOrganizationOptions.OrganizationId is not null)
                queryable = queryable.Where(so => so.OrganizationId == sessionOrganizationOptions.OrganizationId);
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

    public async Task<long> GetSingleOrDefaultOrganizationIdBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null)
    {
        var set = GetDBSet(options);
        var Organization = await set
            .Where(e => e.SessionId == sessionId)
            .Select(e => e.OrganizationId)
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
