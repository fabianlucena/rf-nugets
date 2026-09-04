using RFIRepositories.IRepositories;
using RFRGOBAC.Entities;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.IRepositories;

public interface ISessionOrganizationRepository : INoIdEntityRepository<SessionOrganization>
{
    Task<Organization?> GetSingleOrDefaultOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null);
    Task<long> GetSingleOrDefaultOrganizationIdBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null);
    Task<Organization> GetSingleOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null);
}