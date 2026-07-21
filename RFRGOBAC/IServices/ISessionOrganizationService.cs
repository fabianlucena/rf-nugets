using RFIServices.IServices;
using RFRGOBAC.Entities;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.IServices;

public interface ISessionOrganizationService : INoIdEntityService<SessionOrganization>
{
    Task<Organization> GetSingleOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null);
    Task<Organization?> GetSingleOrDefaultOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null);
}
