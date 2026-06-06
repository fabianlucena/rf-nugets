using RFRGOBAC.Entities;
using RFRGOBAC.IRepositories;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;
using RFServices.Services;

namespace RFRGOBAC.Services;

public class SessionOrganizationService(
    ISessionOrganizationRepository sessionOrganizationRepository
) : NoIdEntityService<SessionOrganization>(sessionOrganizationRepository),
    ISessionOrganizationService
{
    public async Task<Organization> GetSingleOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null)
    {
        var Organization= await sessionOrganizationRepository.GetSingleOrganizationBySessionIdAsync(sessionId, options);
        return Organization;
    }

    public async Task<Organization?> GetSingleOrDefaultOrganizationBySessionIdAsync(long id, SessionOrganizationQueryOptions? options = null)
    {
        var Organization = await sessionOrganizationRepository.GetSingleOrDefaultOrganizationBySessionIdAsync(id, options);
        return Organization;
    }
}
