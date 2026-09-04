using RFRegisterService.Attributes;
using RFRGOBAC.Entities;
using RFRGOBAC.IRepositories;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;
using RFServices.Services;

namespace RFRGOBAC.Services;

[RegisterService]
public class SessionOrganizationService(
    ISessionOrganizationRepository sessionOrganizationRepository,
    IServiceProvider serviceProvider
) : NoIdEntityService<SessionOrganization>(sessionOrganizationRepository, serviceProvider),
    ISessionOrganizationService
{
    public async Task<Organization> GetSingleOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null)
        => await sessionOrganizationRepository.GetSingleOrganizationBySessionIdAsync(sessionId, options);

    public async Task<Organization?> GetSingleOrDefaultOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null)
        => await sessionOrganizationRepository.GetSingleOrDefaultOrganizationBySessionIdAsync(sessionId, options);

    public async Task<long> GetSingleOrDefaultOrganizationIdBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null)
        => await sessionOrganizationRepository.GetSingleOrDefaultOrganizationIdBySessionIdAsync(sessionId, options);
}
